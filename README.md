<p align="center"> <img alt="Legacy of Paradise" width="653" height="256" src="https://github.com/Legacy-Of-Paradise/frontier-erp/blob/master/Resources/Textures/_NewParadise/Logo/logo.png?raw=true" /></p>

Legacy of Paradise — это форк [Space Station 14](https://github.com/space-wizards/space-station-14), работающий на движке [Robust Toolbox](https://github.com/space-wizards/RobustToolbox), написанном на C#.

Это основной репозиторий Legacy Of Paradise Frontier.

Если вы хотите запустить сервер или создавать контент для LOP, то вам нужен именно этот репозиторий. В нём содержится как RobustToolbox, так и набор ресурсов для разработки новых контент-паков.

## Ссылки

[Discord](https://wiki.legacyofparadise.space/discord/) | [Steam](https://store.steampowered.com/app/1255460/Space_Station_14/)

## Документация/Вики

В нашей [вики](https://wiki.legacyofparadise.space/) есть документация по контенту LOP.

## Внесение вклада

Мы рады принять вклад от любого участника. Присоединяйтесь к нашему Discord, если хотите помочь. У нас есть [список идей](https://wiki.legacyofparadise.space/discord/), которые можно реализовать, и любой желающий может их взять. Не бойтесь спрашивать, если вам нужна помощь!

В настоящее время мы не принимаем переводы игры в основном репозитории. Если вы хотите перевести игру на другой язык, рассмотрите возможность создания форка или внесения вклада в существующий форк.

Если вы вносите изменения, пожалуйста, ознакомьтесь с разделом маркеров в [MARKERS.md](https://github.com/Legacy-Of-Paradise/frontier-erp/blob/master/MARKERS.md). Все изменения в файлах, принадлежащих нашему upstream, должны быть должным образом помечены в соответствии с указанными там правилами.

## Сборка проекта.

### Зависимости

> - Git
> - .NET SDK 9.0.x

### Windows

> 1. Клонируйте репо
> 3. Запустите `Scripts/bat/buildAllDebug.bat` после каждого измения в C#.
> 4. Запустите `Scripts/bat/runQuickAll.bat` для запуска клиента и сервера.
> 5. Подключитесь к "localhost" в клиенте.

### Linux

> 1. Клонируйте репо
> 3. Запустите `Scripts/sh/buildAllDebug.sh` после каждого изменения в C#.
> 4. Запустите `Scripts/sh/runQuickAll.sh` для запуска клиента и сервера.
> 5. Подключитесь к "localhost" в клиенте.

### Основные решения проблем

Попробуйте удалить папку bin и внутренности RobustToolBox.

## Лицензия

Контент, добавленный в этот репозиторий после коммита `2fca06eaba205ae6fe3aceb8ae2a0594f0effee0`, лицензируется в соответствии с GNU Affero General Public License версии 3.0, если не указано иное. См. `LICENSE-AGPLv3.txt`.
Контент, добавленный до коммита `2fca06eaba205ae6fe3aceb8ae2a0594f0effee0`, лицензируется по лицензии MIT, если не указано иное. См. `LICENSE-MIT.txt`.

Коммит [2fca06eaba205ae6fe3aceb8ae2a0594f0effee0](https://github.com/Legacy-Of-Paradise/frontier-erp/commit/2fca06eaba205ae6fe3aceb8ae2a0594f0effee0) был загружен 1 июля 2024 года в 16:04 UTC.

Большинство ресурсов лицензированы под [CC-BY-SA 3.0](https://creativecommons.org/licenses/by-sa/3.0/), если не указано иное. Лицензия и авторские права указаны в метаданных файлов. [Пример](https://github.com/space-wizards/space-station-14/blob/master/Resources/Textures/Objects/Tools/crowbar.rsi/meta.json).

Обратите внимание, что некоторые ресурсы лицензированы под некоммерческими лицензиями, такими как [CC-BY-NC-SA 3.0](https://creativecommons.org/licenses/by-nc-sa/3.0/) или аналогичными, и их необходимо удалить, если вы планируете коммерческое использование проекта.

## Атрибуты


Когда мы берем контент из других форков, мы распределяем его по подкаталогам, специфичным для репо, чтобы лучше отслеживать авторство и ограничить конфликты при слиянии.


Содержимое в этих подкаталогах происходит из соответствующих форков и может содержать изменения. Эти изменения обозначаются комментариями вокруг измененных строк.

| Директория | Название | Ссылка | Лицензия |
|------------|----------|--------|----------|
| `_NF` | Frontier Station | https://github.com/new-frontiers-14/frontier-station-14 | AGPL 3.0 |
| `_CD` | Cosmatic Drift | https://github.com/cosmatic-drift-14/cosmatic-drift | MIT |
| `_Corvax` | Corvax | https://github.com/space-syndicate/space-station-14 | MIT |
| `_Corvax` | Corvax Frontier | https://github.com/Corvax-Frontier/Frontier | AGPL 3.0 |
| `_DV` | Delta-V | https://github.com/DeltaV-Station/Delta-v | AGPL 3.0 |
| `_EE` | Einstein Engines | https://github.com/Simple-Station/Einstein-Engines | AGPL 3.0 |
| `_Emberfall` | Emberfall | https://github.com/emberfall-14/emberfall | MPL 2.0 |
| `_EstacaoPirata` | Estacao Pirata | https://github.com/Day-OS/estacao-pirata-14 | AGPL 3.0 |
| `_Goobstation` | Goob Station | https://github.com/Goob-Station/Goob-Station | AGPL 3.0 |
| `_Impstation` | Impstation | https://github.com/impstation/imp-station-14 | AGPL 3.0 |
| `_NC14` | Nuclear 14 | https://github.com/Vault-Overseers/nuclear-14 | AGPL 3.0 |
| `Nyanotrasen` | Nyanotrasen | https://github.com/Nyanotrasen/Nyanotrasen | MIT |

Ниже перечислены дополнительные репозитории, из которых мы перенесли функции без подкаталогов.

| Название | Ссылка | Лицензия |
|----------|--------|----------|
| Space Station 14 | https://github.com/space-wizards/space-station-14 | MIT |
| White Dream | https://github.com/WWhiteDreamProject/wwdpublic | AGPL 3.0 |

![Alt](https://repobeats.axiom.co/api/embed/f3b7ade55d5c177fcfbfc914d75d97bef17c175b.svg "Repobeats analytics image")
