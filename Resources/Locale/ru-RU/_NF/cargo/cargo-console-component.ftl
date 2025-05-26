## UI

cargo-console-menu-nf-populate-orders-cargo-order-row-product-name-text = { CAPITALIZE($productName) } (x{ $total }) для { $purchaser }
cargo-console-menu-nf-populate-orders-cargo-order-row-product-quantity-text = Осталось { $remaining }
cargo-console-menu-nf-order-capacity = { $count }/{ $capacity }
cargo-console-order-nf-menu-notes-label = Примечания:

## Orders

cargo-console-nf-no-bank-account = Банковский счёт не найден
cargo-console-nf-paper-print-text = [head=2]Заказ #{ $orderNumber }[/head]
    { "[bold]Предмет:[/bold]" } { $itemName } ({ $orderIndex } из { $orderQuantity })
    { "[bold]Приобретено:[/bold]" } { $purchaser }
    { "[bold]Примечания:[/bold]" } { $notes }

## Upgrades

cargo-telepad-delay-upgrade = Задержка телепортации
