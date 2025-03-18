mail-recipient-mismatch = Имя или должность получателя не совпадают.
mail-recipient-mismatch-name = Имя получателя не совпадает.
mail-invalid-access = Имя и должность получателя совпадают, но доступ не соответствует ожиданиям.
mail-locked = Защитный замок не был снят. Приложите ID получателя.
mail-desc-far = Почтовая посылка.
mail-desc-close = Почтовая посылка, адресованная { CAPITALIZE($name) }, { $job }. Последнее известное местоположение: { $station }.
mail-desc-fragile = На ней есть [color=red]красная наклейка "Хрупкое"[/color].
mail-desc-priority = Защитный замок активирован [color=yellow]жёлтой приоритетной лентой[/color].
mail-desc-priority-inactive = Защитный замок неактивен, [color=#886600]жёлтая приоритетная лента[/color] отсутствует.
mail-unlocked = Защитная система разблокирована.
mail-unlocked-by-emag = Защитная система *БZZT*.
mail-unlocked-reward = Защитная система разблокирована. На счёт Frontier добавлено { $bounty } spesos.
mail-penalty-lock = ЗАЩИТНЫЙ ЗАМОК ВЗЛОМАН. БАНКОВСКИЙ СЧЁТ СТАНЦИИ ОШТРАФОВАН НА { $credits } SPESOS.
mail-penalty-fragile = ЦЕЛОСТНОСТЬ НАРУШЕНА. БАНКОВСКИЙ СЧЁТ СТАНЦИИ ОШТРАФОВАН НА { $credits } SPESOS.
mail-penalty-expired = ДОСТАВКА ПРОСРОЧЕНА. БАНКОВСКИЙ СЧЁТ СТАНЦИИ ОШТРАФОВАН НА { $credits } SPESOS.
mail-item-name-unaddressed = почта
mail-item-name-addressed = почта ({ $recipient })
# Frontier: reworded description, does not need to be a container.
command-mailto-description = Запланировать отправку предмета получателю. Пример использования: `mailto 1234 5678 false false`. Если целевая сущность является контейнером, её содержимое будет перемещено в почтовую посылку.
# Frontier: add is-large description, container<contents
command-mailto-help = Использование: { $command } <recipient entityUid> <contents entityUid> [is-fragile: true|false] [is-priority: true|false] [is-large: true|false]
command-mailto-no-mailreceiver = Целевая сущность получателя не содержит { $requiredComponent }.
command-mailto-no-blankmail = Прототип { $blankMail } не существует. Что-то пошло не так. Обратитесь к программисту.
command-mailto-bogus-mail = { $blankMail } не содержит { $requiredMailComponent }. Что-то пошло не так. Обратитесь к программисту.
command-mailto-invalid-container = Целевая сущность контейнера не содержит { $requiredContainer } контейнер.
command-mailto-unable-to-receive = Целевая сущность получателя не может принимать почту. Возможно, отсутствует ID.
command-mailto-no-teleporter-found = Целевая сущность получателя не сопоставлена ни с одним почтовым телепортером станции. Получатель может находиться за пределами станции.
command-mailto-success = Успешно! Почтовая посылка будет телепортирована через { $timeToTeleport } секунд.
# Frontier: mailto command completions
command-mailto-completion-recipient = <recipient entityUid>
command-mailto-completion-container = <contents entityUid>
command-mailto-completion-fragile = [is-fragile: true|false]
command-mailto-completion-priority = [is-priority: true|false]
command-mailto-completion-large = [is-large: true|false]

# End Frontier

command-mailnow = Принудительно заставить все почтовые телепортеры доставить ещё один раунд почты как можно скорее. Это не обойдёт лимит недоставленных посылок.
command-mailnow-help = Использование: { $command }
command-mailnow-success = Успешно! Все почтовые телепортеры скоро совершат ещё одну доставку.
