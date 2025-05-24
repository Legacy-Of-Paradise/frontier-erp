ore-silo-ui-nf-itemlist-entry =
    { $linked ->
        [true] { "[Подключено] " }
       *[False] { "" }
    } { $name } { $inRange ->
        [true] { "" }
       *[false] (Вне зоны действия)
    }
