# View: связь сущностей с GameObject

## EntityInitializer и binds

- `EntityInitializer` на корне view: `Bind(entity, world)` вызывает `IEntityBind.Bind` у всех дочерних binds, `Unbind()` — `Unbind`.
- Binds кладут в сущность ссылки на объекты сцены и префаба:
  - `RigidbodyBind` → `RigidbodyComponent`;
  - `BillboardBind` → `Billboard { Transform }`.
- Правило: значения из ассетов идут через трейты, ссылки на объекты сцены — через binds.

## Откуда берётся view

- **Префаб**: `ViewTrait` → `AssetPath`; `CreateViewSystem` (Update, View 100) инстанцирует префаб через `IAssetLoader.LoadImmediate` в `Position`, ставит `View` и вызывает `Bind`.
- **Объект сцены**: `AgentAuthoring` (`AgentConfig`) + `EntityInitializer`. `SceneAgentSpawnInitializer` создаёт сущность, ставит `View` и вызывает `Bind`. `CreateViewSystem` такие сущности пропускает (`Without<View>`).

## Системы группы View (100)

| Тип | Система | Что делает |
|---|---|---|
| Update | `CreateViewSystem` | Создание view из `AssetPath` |
| Update | `ViewToPositionSystem` | `Position = view.transform.position` — источник позиции для восприятия, навигации и ИИ |
| Late | `BillboardSystem` | Поворачивает `Billboard.Value` к камере вокруг вертикали |
| Cleanup | `DestroyViewSystem` | По `DestroySelfRequest`: `Unbind` + уничтожение GameObject. При `Dispose` мира — все view |

## Спрайтовые враги

Враг — капсула-коллайдер + Rigidbody (`FreezeRotation`) + дочерний `Sprite` (`SpriteRenderer`) с `BillboardBind`. Логическое направление взгляда (`FacingDirection`) хранится в ECS и визуально пока не отображается.

План — 8-направленные спрайты, как в оригинальном Daggerfall: угол между `FacingDirection` и направлением на камеру выбирает кадр.
