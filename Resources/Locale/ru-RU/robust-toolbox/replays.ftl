# Playback Commands

cmd-replay-play-desc = Возобновить воспроизведение записи.
cmd-replay-play-help = replay_play
cmd-replay-pause-desc = Приостановить воспроизведение записи.
cmd-replay-pause-help = replay_pause
cmd-replay-toggle-desc = Возобновить или приостановить воспроизведение записи.
cmd-replay-toggle-help = replay_toggle
cmd-replay-stop-desc = Остановить и выгрузить запись.
cmd-replay-stop-help = replay_stop
cmd-replay-load-desc = Загрузить и начать воспроизведение записи.
cmd-replay-load-help = replay_load <папка записи>
cmd-replay-load-hint = Папка записи
cmd-replay-skip-desc = Пропустить вперёд или назад во времени.
cmd-replay-skip-help = replay_skip <тик или промежуток времени>
cmd-replay-skip-hint = Тики или промежуток времени (ЧЧ:ММ:СС).
cmd-replay-set-time-desc = Перейти вперёд или назад к определённому времени.
cmd-replay-set-time-help = replay_set <тик или время>
cmd-replay-set-time-hint = Тик или промежуток времени (ЧЧ:ММ:СС), начиная с
cmd-replay-error-time = "{ $time }" не является целым числом или промежутком времени.
cmd-replay-error-args = Неверное количество аргументов.
cmd-replay-error-no-replay = В данный момент запись не воспроизводится.
cmd-replay-error-already-loaded = Запись уже загружена.
cmd-replay-error-run-level = Вы не можете загрузить запись, будучи подключенным к серверу.

# Recording commands

cmd-replay-recording-start-desc = Начинает запись повтора, опционально с ограничением по времени.
cmd-replay-recording-start-help = Использование: replay_recording_start [имя] [перезаписать] [лимит времени]
cmd-replay-recording-start-success = Начата запись повтора.
cmd-replay-recording-start-already-recording = Запись повтора уже идёт.
cmd-replay-recording-start-error = Произошла ошибка при попытке начать запись.
cmd-replay-recording-start-hint-time = [лимит времени (минуты)]
cmd-replay-recording-start-hint-name = [имя]
cmd-replay-recording-start-hint-overwrite = [перезаписать (булево)]
cmd-replay-recording-stop-desc = Останавливает запись повтора.
cmd-replay-recording-stop-help = Использование: replay_recording_stop
cmd-replay-recording-stop-success = Запись повтора остановлена.
cmd-replay-recording-stop-not-recording = В данный момент запись не ведётся.
cmd-replay-recording-stats-desc = Отображает информацию о текущей записи повтора.
cmd-replay-recording-stats-help = Использование: replay_recording_stats
cmd-replay-recording-stats-result = Длительность: { $time } мин, Тиков: { $ticks }, Размер: { $size } МБ, Скорость: { $rate } МБ/мин.

# Time Control UI

replay-time-box-scrubbing-label = Динамическая очистка
replay-time-box-replay-time-label = Время записи: { $current } / { $end }  ({ $percentage }%)
replay-time-box-server-time-label = Серверное время: { $current } / { $end }
replay-time-box-index-label = Индекс: { $current } / { $total }
replay-time-box-tick-label = Тик: { $current } / { $total }
