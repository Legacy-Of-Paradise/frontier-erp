# Вклад в Legacy of Paradise

Если вы рассматриваете возможность внести свой вклад в Legacy of Paradise, [руководство Wizard's Den по PR](https://docs.spacestation14.com/en/general-development/codebase-info/pull-request-guidelines.html) является хорошей отправной точкой для понимания качества кода и этикета отслеживания версий. Обратите внимание, что у нас нет такого разделения на ветки master/stable.

Важно: **не вносите изменения через веб-редактор**. Из текста выше:

> Не используйте веб-редактор GitHub для создания PR. PR, отправленные через веб-редактор, могут быть закрыты без рассмотрения.

"Upstream" относится к репозиторию [space-wizards/space-station-14](https://github.com/space-wizards/space-station-14), от которого был создан этот форк.

# Специфичный для LOP контент

В целом, все, что вы создаете с нуля (в отличие от изменения существующего контента из upstream), должно находиться в специфичной для Legacy Of Paradise подпапке `_NewParadise`.

Примеры:

- `Content.Server/_NewParadise/Shipyard/Systems/ShipyardSystem.cs`
- `Resources/Prototypes/_NewParadise/Loadouts/role_loadouts.yml`
- `Resources/Audio/_NewParadise/Voice/Goblin/goblin-scream-03.ogg`
- `Resources/Textures/_NewParadise/Tips/clippy.rsi/left.png`
- `Resources/Locale/en-US/_NewParadise/devices/pda.ftl`
- `Resources/ServerInfo/_NewParadise/Guidebook/Medical/Doc.xml`

# Изменения в файлах upstream

Если вы вносите изменения в C# или YAML-файл из upstream, **вы обязаны добавить комментарии на измененных строках или рядом с ними**.

Комментарии должны пояснять, что именно было изменено, чтобы упростить разрешение конфликтов при изменении файла в upstream.

Если вы изменяете значения, для единообразия оставляйте комментарий в формате `LOP: OLD<NEW`.

Для YAML в частности: если вы добавляете компонент или список смежных полей, используйте блочные комментарии, но если вы вносите ограниченные изменения в поля компонента, комментируйте поля индивидуально.

Для C# файлов: если вы добавляете большой объем кода, рассмотрите возможность использования разделяемых классов (partial class), если это имеет смысл.

При заимствовании (cherry-picking) функций из upstream лучше всего комментировать номером PR, который был заимствован.

Кроме того, fluent (.ftl) файлы **не поддерживают комментарии на той же строке**, что и значение локализации - оставляйте комментарий на строке выше при изменении значений.

## Примеры комментариев в файлах upstream или портированных файлах

Однострочный комментарий к измененному полю yml:

```yml
- type: entity
  id: TorsoHarpy
  name: "harpy torso"
  parent: [PartHarpy, BaseTorso] # LOP edit: add BaseTorso
```

Изменение значения (обратите внимание на формат `OLD<NEW`):

```yml
  - type: Gun
    fireRate: 4 # LOP edit: 3<4
    availableModes:
    - SemiAuto
```

Модуль киборга с добавленным полем moduleId (встроенный пустой комментарий), закомментированным ведром (встроенный пустой комментарий) и DroppableBorgModule, который мы добавили (начало/конец блочного комментария):

```yml
  - type: ItemBorgModule
    moduleId: Gardening # LOP edit
    items:
    - HydroponicsToolMiniHoe
    - HydroponicsToolSpade
    - HydroponicsToolClippers
    # - Bucket # LOP edit
  # LOP edit start: droppable borg items
  - type: DroppableBorgModule
    moduleId: Gardening
    items:
    - id: Bucket
      whitelist:
        tags:
        - Bucket
  # LOP edit end
```

Комментарий к новому импортированному пространству имен:

```cs
using Content.Client._NF.Emp.Overlays; // LOP edit
```

Пара комментариев, заключающих блок добавленного кода:

```cs
component.Capacity = state.Capacity;

component.UIUpdateNeeded = true;

// LOP edit start: ensure signature colour is consistent
if (TryComp<StampComponent>(uid, out var stamp))
{
    stamp.StampedColor = state.Color;
}
// LOP edit end
```

# Картостроение

Для отправки кораблей ознакомьтесь с [Руководством по отправке кораблей](https://frontierstation.wiki.gg/wiki/Ship_Submission_Guidelines) на вики Frontier.

В общем:

Frontier использует специфические прототипы для точек интереса и карт кораблей (например, для хранения информации о спавне, данных о спавне станций или цен и категорий кораблей). Для кораблей они хранятся в VesselPrototype (Resources/Prototypes/_NewParadise/Shipyard) или PointOfInterestPrototype (Resources/Prototypes/_NewParadise/PointsOfInterest). При создании нового корабля или POI обратитесь к существующим прототипам.

Если вы вносите изменения в карту, свяжитесь с мейнтейнером карты (или, если его нет, с автором) и избегайте наличия нескольких открытых PR с изменениями одной и той же карты.

Конфликты с картами делают PR взаимоисключающими, поэтому либо ваша работа, либо работа мейнтейнера будет потеряна. Общайтесь, чтобы избежать этого!

# Перед отправкой

Дважды проверьте свой diff на GitHub перед отправкой: поищите непреднамеренные коммиты или изменения и удалите случайные пробелы или изменения окончания строк.

# Журналы изменений (Changelogs)

В настоящее время все журналы изменений попадают в журнал изменений LOP. Префикс `ADMIN:` в данный момент ничего не делает.

# Дополнительные ресурсы

Если вы новичок в разработке в SS14 в целом, ознакомьтесь с [документацией SS14](https://docs.spacestation14.io/) или обратитесь за помощью в `#dev-help` на [Discord](https://wiki.legacyofparadise.space/)!

## Контент, сгенерированный ИИ

Код, спрайты и любой другой контент, сгенерированный ИИ, не разрешается отправлять в репозиторий.

Попытка отправить PR с контентом, сгенерированным ИИ, может привести к блокировке вашей возможности вносить вклад.
