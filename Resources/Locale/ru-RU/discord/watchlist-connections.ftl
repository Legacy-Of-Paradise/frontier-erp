discord-watchlist-connection-header =
    { $players } { $players ->
        [one] игрок в списке наблюдения подключился
        [few] игрока в списке наблюдения поключились
       *[other] игроков в списке наблюдения подключились
    } к { $serverName }
discord-watchlist-connection-entry =
    - { $playerName } с сообщением "{ $message }"{ $expiry ->
        [0] { "" }
       *[other] { " " }(истекает <t:{ $expiry }:R>)
    }{ $otherWatchlists ->
        [0] { "" }
        [one] { " " }и еще { $otherWatchlists } наблюдением
        [few] { " " }и еще { $otherWatchlists } наблюдения
       *[other] { " " }и еще { $otherWatchlists } наблюдений
    }
