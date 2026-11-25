# Бой

## Модель

Оружие — **отдельная сущность**. Агент держит `EquippedWeapon { Entity }`, оружие — `WeaponOwner { Entity }`. Оружие создаёт `WeaponHolderTrait` на агенте из `WeaponConfig`.

### Трейты оружия (`IWeaponTrait`)

| Трейт | Компонент | Параметры по умолчанию |
|---|---|---|
| `AttackTimingTrait` | `AttackTiming` | замах 0.3 с, момент удара 0.45 (доля замаха), восстановление 0.35 с |
| `WeaponDamageTrait` | `WeaponDamage` | базовый урон 10 |
| `ChargeTrait` | `ChargeAttack` | заряд до 1.5 с, урон при полном заряде 30. **Наличие трейта = атака заряжаемая** |
| `MeleeHitTrait` | `MeleeHit` | дальность 2, радиус 0.3, маска слоёв |
| `WeaponPresentationTrait` | `WeaponPresentationRef` | ссылка на ассет `WeaponPresentation` (спрайт, смещения и повороты для оверлея) |

### Агент

`WeaponHolderTrait` (`_startingWeapon`) ставит `AttackIntent { Held }`, `AimRay { Origin, Direction }` и `ActionBlockers`, создаёт сущность оружия.

`AimRay` игрока заполняет `CameraAimSystem` из камеры. Врагам его будет заполнять ИИ. Melee не знает о камере.

## Фазы атаки

Фаза определяется наличием компонента на оружии:
- нет компонента — покой;
- `AttackCharging { Elapsed }`;
- `AttackSwing { Elapsed, Charge, HitResolved }`;
- `AttackRecovery { Elapsed }`;
- одноразовый `AttackHit { Damage }`.

## Системы (группа Combat 700, Update)

Системы фаз идут в **обратном порядке переходов**: новая фаза начинает отсчёт со следующего кадра и не теряет время.

1. `CameraAimSystem` — `AimRay` игрока из камеры.
2. `AttackInterruptSystem` — если у владельца подняты `ActionBlockers`, снимает все фазы и `AttackHit`.
3. `AttackRecoverySystem` — после `RecoveryDuration` возвращает оружие в покой.
4. `AttackSwingSystem`:
   - на `HitTime` ставит `AttackHit` с уроном `lerp(base, max(base, maxCharged), charge)`;
   - в конце замаха переводит в `AttackRecovery`.
5. `MeleeHitSystem` — `SphereCast` по `AimRay` владельца.
   - Попадание определяется через `GetComponentInParent<EntityInitializer>()`. Владелец и чужие миры пропускаются.
   - Создаёт сущность `DamageEvent`.
6. `AttackChargeSystem` — копит заряд. Когда `AttackIntent.Held` отпущен — `AttackSwing { Charge }`.
7. `AttackStartSystem` — по `AttackRequest` владельца при пустых `ActionBlockers` и оружии в покое: `AttackCharging`, если есть `ChargeAttack`, иначе `AttackSwing`.
8. `WeaponPresentationSystem` — публикует `WeaponPresentationMessage` для экипированного оружия игрока.
9. `ComponentCleanupSystem<AttackHit>`, `ComponentCleanupSystem<AttackRequest>`.
10. `WeaponOwnerDestroySystem` (cleanup) — удаляет оружие вместе с владельцем.

## Оверлей оружия

`WeaponOverlay` (Canvas) слушает `WeaponPresentationMessage` и считает позу спрайта по фазе и прогрессу. Сам ничего не тикает: тайминг атаки только в ECS.

## Известные ограничения

- Маска попаданий — по слоям. Тестовое оружие исключает слой `Player` (6), поэтому враг с таким оружием не попадёт по игроку. План — фракции.
- Одно оружие на агента, смены оружия пока нет.
- Атака во время даша не блокируется и не буферизуется.
