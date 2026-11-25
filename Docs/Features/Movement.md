# Ввод и движение

## Ввод игрока (`Gameplay/Input`)

- `PlayerControlTrait` → `PlayerControlled`, `InputMoveDirection`.
- Инициализаторы подписываются на `IInputSystem.InputEvents` (R3 `Subject`) и пишут в сущности с `PlayerControlled`:
  - `MoveInputApplyInitializer` → `InputMoveDirection.Value` (Vector2);
  - `DashInputApplyInitializer` → `DashRequest`;
  - `AttackInputApplyInitializer`: `Started` → `AttackIntent.Held = true` + `AttackRequest`, `Canceled` → `Held = false`.
- Колбэки Input System приходят до Update, поэтому запросы видны системам в том же кадре.

ИИ пишет те же компоненты (`MoveDirection`, `AttackRequest`, `DashRequest`), что и ввод. Исполняющим системам всё равно, кто источник.

## Компоненты движения

| Компонент | Смысл |
|---|---|
| `MoveSpeed` | Скорость, м/с |
| `MoveDirection` | Желаемое направление в мире (длина ≤ 1) |
| `Velocity` | Горизонтальная скорость текущего fixed-тика (собирается заново каждый тик) |
| `FacingDirection` | Логическое направление взгляда (для FOV, даша, будущих 8-направленных спрайтов) |
| `MovementBlockers` | Флаги `MovementBlockReason`: `Dash`, `Dead` |
| `CanMove` | Тег: блокировок нет (ставит `MovementAvailabilitySystem`) |
| `RigidbodyComponent` | Ссылка на Rigidbody (через `RigidbodyBind`) |

`MovementTrait` (`_moveSpeed`, по умолчанию 5) ставит `MoveSpeed`, `MovementBlockers`, `Velocity`, `MoveDirection` и `FacingDirection = forward`.

## Конвейер скорости

| Группа | Тип | Система | Что делает |
|---|---|---|---|
| VelocityReset 300 | Fixed | `ResetVelocitySystem` | `Velocity = 0` |
| Dash 400 | Fixed | см. `Dash.md` | добавляет скорость даша |
| Movement 500 | Fixed | `MovementAvailabilitySystem` | `CanMove` по `MovementBlockers` |
| Movement 500 | Fixed | `MoveDirectionToVelocitySystem` | `Velocity += MoveSpeed * MoveDirection` (только с `CanMove`) |
| VelocityApply 600 | Fixed | `VelocityToRigidbodySystem` | `linearVelocity = (Velocity.x, rb.y, Velocity.z)` — вертикаль остаётся физике |

Системы группы Movement, работающие в Update:
- `CameraRelativeInputToMoveDirectionSystem` — `InputMoveDirection` → `MoveDirection` в плоском базисе камеры (`CameraPlanarBasis`);
- `CameraFacingSystem` — `FacingDirection` игрока = плоский forward камеры;
- `FaceMoveDirectionSystem` — `FacingDirection` не-игроков = направление движения (если оно есть).

## Камера

`CinemachineCamera` (PanTilt, InputAxisController, шум) — дочерний объект `PlayerAvatar` на позиции `(0, 1.32, 0)`, **на оси капсулы**. Капсула не вращается (`FreezeRotation`), вращается только камера. Если сместить камеру с оси, near plane начнёт выходить за пределы капсулы и камера будет проваливаться в стены. Near plane — 0.1, радиус капсулы — 0.5.

## Физический материал агентов

Капсулы агентов используют `Content/Materials/Physics/AgentFrictionless` (трение 0, combine `Minimum`). С трением по умолчанию (0.6) скорость, которую мы каждый тик задаём в стену, прижимала капсулу, и трение не давало ни скользить вдоль стены, ни падать: агент «прилипал». Горизонтальная скорость всё равно задаётся каждый тик, поэтому на ровном полу без трения агент не скользит.

Ограничение: на наклонных поверхностях агент без трения будет медленно сползать вниз под действием гравитации. Когда появятся склоны — нужна проекция скорости на нормаль земли или отдельный расчёт «стоит на земле».

Новым агентам (префабам и объектам сцены) назначай этот материал на капсулу.

## Точки расширения

- Новый источник скорости (отбрасывание, рывок атаки) — отдельная fixed-группа между 300 и 600 со своим флагом в `MovementBlockers`, если он должен подавлять обычное движение.
- Замедление при заряде атаки — модификатор `MoveSpeed`. Пока не сделано.
