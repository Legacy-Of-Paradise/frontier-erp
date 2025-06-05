ore-silo-ui-title = Отсек для материалов
ore-silo-ui-label-clients = Машины
ore-silo-ui-label-mats = Материалы
ore-silo-ui-itemlist-entry =
    { $linked ->
        [true] { "[Подключено] " }
       *[False] { "" }
    } { $name } ({ $beacon }) { $inRange ->
        [true] { "" }
       *[false] (Вне зоны действия)
    }
