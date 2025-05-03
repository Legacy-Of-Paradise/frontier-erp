#!/usr/bin/env python3

#
# Sends updates to a Discord webhook for new changelog entries since the last GitHub Actions publish run.
# Automatically figures out the last run and changelog contents with the GitHub API.
#

import io
import itertools
import os
import requests
import yaml
from typing import Any, Iterable

GITHUB_API_URL     = os.environ.get("GITHUB_API_URL", "https://api.github.com")
GITHUB_REPOSITORY = os.environ["GITHUB_REPOSITORY"]
GITHUB_RUN        = os.environ["GITHUB_RUN_ID"]
GITHUB_TOKEN      = os.environ["GITHUB_TOKEN"]
CHANGELOG_DIR     = os.environ["CHANGELOG_DIR"]
CHANGELOG_WEBHOOK = os.environ["CHANGELOG_WEBHOOK"]

# https://discord.com/developers/docs/resources/webhook
DISCORD_SPLIT_LIMIT = 1950  # Немного уменьшим лимит, чтобы учесть возможные накладные расходы

TYPES_TO_EMOJI = {
    "Fix":    "🐛",
    "Add":    "✨",
    "Remove": "❌",
    "Tweak":  "⚒️"
}

ChangelogEntry = dict[str, Any]

def main():
    if not CHANGELOG_WEBHOOK:
        return

    session = requests.Session()
    session.headers["Authorization"]     = f"Bearer {GITHUB_TOKEN}"
    session.headers["Accept"]            = "Accept: application/vnd.github+json"
    session.headers["X-GitHub-Api-Version"] = "2022-11-28"

    most_recent = get_most_recent_workflow(session)
    if most_recent and 'head_commit' in most_recent and 'id' in most_recent:
        last_sha = most_recent['head_commit']['id']
        print(f"Last successful publish job was {most_recent['id']}: {last_sha}")
        last_changelog = yaml.safe_load(get_last_changelog(session, last_sha))
        with open(CHANGELOG_DIR, "r") as f:
            cur_changelog = yaml.safe_load(f)

        diff = diff_changelog(last_changelog, cur_changelog)
        send_to_discord(diff)
    else:
        print("Warning: Could not determine the last successful workflow run.")


def get_most_recent_workflow(sess: requests.Session) -> Any:
    workflow_run = get_current_run(sess)
    if not workflow_run:
        print("Error: Could not get current workflow run information.")
        return None

    print(f"Current workflow run ID: {workflow_run['id']}")

    past_runs = get_past_runs(sess, workflow_run)
    if not past_runs or 'workflow_runs' not in past_runs:
        print("Warning: Could not retrieve past workflow runs.")
        return None

    print(f"Number of past workflow runs found: {len(past_runs['workflow_runs'])}")

    most_recent_successful_run = None
    for run in past_runs['workflow_runs']:
        print(f"Checking past run ID: {run['id']}, Status: {run.get('status')}, Conclusion: {run.get('conclusion')}")
        if run["id"] == workflow_run["id"]:
            print("Skipping current workflow run.")
            continue
        # Проверяем статус и conclusion (если есть) для определения успешного запуска
        if run.get("status") == "completed" and run.get("conclusion") == "success":
            most_recent_successful_run = run
            print(f"Found most recent successful run ID: {most_recent_successful_run['id']}")
            break # Нашли первый успешный и более ранний, можно выходить
        elif run.get("status") == "success": # Дополнительная проверка на случай, если conclusion отсутствует
            most_recent_successful_run = run
            print(f"Found most recent successful run ID (by status): {most_recent_successful_run['id']}")
            break

    if most_recent_successful_run:
        print(f"Most recent successful workflow run: {most_recent_successful_run['id']}")
        return most_recent_successful_run
    else:
        print("Warning: No previous successful workflow run found with status 'completed' and conclusion 'success' (or status 'success').")
        return None


def get_current_run(sess: requests.Session) -> Any:
    resp = sess.get(f"{GITHUB_API_URL}/repos/{GITHUB_REPOSITORY}/actions/runs/{GITHUB_RUN}")
    resp.raise_for_status()
    data = resp.json()
    print(f"get_current_run response: {data}") # Добавлено для отладки
    return data


def get_past_runs(sess: requests.Session, current_run: Any) -> Any:
    params = {
        "status": "completed", # Получаем все завершенные, чтобы проверить conclusion
        "created": f"<={current_run['created_at']}"
    }
    resp = sess.get(f"{current_run['workflow_url']}/runs", params=params)
    resp.raise_for_status()
    data = resp.json()
    print(f"get_past_runs response: {data}") # Добавлено для отладки
    if 'workflow_runs' in data:
        runs_info = [(run.get('id'), run.get('status'), run.get('conclusion')) for run in data['workflow_runs']]
        print(f"Contents of past_runs['workflow_runs']: {runs_info}") # Исправленный вывод
    return data


def get_last_changelog(sess: requests.Session, sha: str) -> str:
    """
    Use GitHub API to get the previous version of the changelog YAML (Actions builds are fetched with a shallow clone)
    """
    params = {
        "ref": sha,
    }
    headers = {
        "Accept": "application/vnd.github.raw"
    }

    resp = sess.get(f"{GITHUB_API_URL}/repos/{GITHUB_REPOSITORY}/contents/{CHANGELOG_DIR}", headers=headers, params=params)
    resp.raise_for_status()
    return resp.text


def diff_changelog(old: dict[str, Any], cur: dict[str, Any]) -> Iterable[ChangelogEntry]:
    """
    Find all new entries not present in the previous publish.
    """
    if not old or not old.get("Entries"):
        return (e for e in cur.get("Entries", []))
    old_entry_ids = {e["id"] for e in old["Entries"]}
    return (e for e in cur.get("Entries", []) if e["id"] not in old_entry_ids)


def get_discord_body(content: str):
    return {
        "content": content,
        # Do not allow any mentions.
        "allowed_mentions": {
            "parse": []
        },
        # SUPPRESS_EMBEDS
        "flags": 1 << 2
    }


def send_discord(content: str):
    body = get_discord_body(content)
    print(f"Отправляю в Discord: {body}")  # Добавлено для отладки
    response = requests.post(CHANGELOG_WEBHOOK, json=body)
    try:
        response.raise_for_status()
        print("Сообщение успешно отправлено в Discord.")
    except requests.exceptions.HTTPError as e:
        print(f"Ошибка при отправке в Discord: {e}")
        print(f"Ответ Discord: {response.text}") # Добавлено для просмотра ответа Discord


def send_to_discord(entries: Iterable[ChangelogEntry]) -> None:
    if not CHANGELOG_WEBHOOK:
        print(f"No discord webhook URL found, skipping discord send")
        return

    message_content = io.StringIO()
    for name, group in itertools.groupby(entries, lambda x: x["author"]):
        group_content = io.StringIO()
        group_content.write(f"## {name}:\n")

        for entry in group:
            for change in entry["changes"]:
                emoji = TYPES_TO_EMOJI.get(change['type'], "❓")
                message = change['message']
                url = entry.get("url")
                if url and url.strip():
                    group_content.write(f"{emoji} - [{message}]({url})\n")
                else:
                    group_content.write(f"{emoji} - {message}\n")

        group_text = group_content.getvalue()
        message_text = message_content.getvalue()
        message_length = len(message_text)
        group_length = len(group_text)

        if message_length + group_length + len("\n") >= DISCORD_SPLIT_LIMIT: # Учитываем разделитель между группами
            if message_length > 0:
                print("Разделяю и отправляю часть changelog в Discord")
                send_discord(message_text)
                message_content = io.StringIO() # Начинаем новое сообщение
            message_content.write(group_text + "\n") # Добавляем текущую группу в новое сообщение
        else:
            message_content.write(group_text + "\n")

    # Отправляем оставшуюся часть
    final_message = message_content.getvalue()
    if len(final_message) > 0:
        print("Отправляю финальную часть changelog в Discord")
        send_discord(final_message.rstrip("\n")) # Удаляем лишний перенос строки в конце


main()
