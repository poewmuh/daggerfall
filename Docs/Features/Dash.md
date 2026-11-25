# Даш

## Компоненты

| Компонент | Поля | Смысл |
|---|---|---|
| `DashParameters` | `Distance`, `Duration`, `Cooldown`, `FallbackDirection`, `Invulnerable` | Параметры из трейта |
| `DashRequest` | — | Одноразовый запрос (от ввода или ИИ) |
| `Dashing` | `Velocity`, `TimeRemaining` | Даш идёт |
| `DashCooldown` | `Remaining` | Идёт кулдаун |

`DashTrait`: дистанция 4, длительность 0.2, кулдаун 1, `FallbackDirection = Facing`, неуязвимость включена. Трейт гарантирует наличие `ActionBlockers`.

## Системы (группа Dash 400, все Fixed)

1. `DashCooldownSystem` — тикает и снимает `DashCooldown`.
2. `DashStartSystem` — фильтр: `DashRequest` + `ActionBlockers` и нет `Dashing` / `DashCooldown`. Если `ActionBlockers` пусты:
   - направление = плоский `MoveDirection`, иначе `FacingDirection` (или обратное при `Backward`);
   - ставит `Dashing { Velocity = dir * Distance / Duration }`;
   - флаг `MovementBlockReason.Dash`;
   - флаг `DamageImmunityReason.Dash`, если `Invulnerable`.
3. `DashVelocitySystem` — `Velocity += Dashing.Velocity * clamp01(TimeRemaining / dt)`. Последний тик берётся долей, поэтому дистанция точная при любом шаге физики.
4. `DashEndSystem` — уменьшает `TimeRemaining`. Завершает даш по времени или если подняты `ActionBlockers` (смерть). При завершении снимает флаги и ставит `DashCooldown`, если кулдаун больше 0.
5. `FixedComponentCleanupSystem<DashRequest>`.

## Важное

- Кулдаун считается от **конца** даша.
- Порядок «кулдаун → старт» нужен, чтобы истёкший кулдаун позволил дашнуть в том же тике.
- `DashRequest` чистится именно в fixed-фазе. В кадре без fixed-тика запрос иначе потерялся бы.
- Запрос, пришедший во время кулдауна, сбрасывается: случайного буфера нет.

## Точки расширения

- Буфер ввода: время жизни у `DashRequest` вместо немедленной очистки.
- Заряды: заменить `DashCooldown` на `DashCharges`. Затронет только `DashStartSystem` и `DashEndSystem`.
