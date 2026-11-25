# Урон, здоровье, смерть

## Компоненты

| Компонент | Смысл |
|---|---|
| `Health { Current, Max }` | Здоровье (`HealthTrait`, по умолчанию 100) |
| `DamageImmunity { Value }` | Флаги `DamageImmunityReason` (`Dash`). Ставится `HealthTrait` |
| `DamageEvent { Target, Source, Amount, Point, Direction }` | **Сущность-событие**, живёт один кадр |
| `Dead` | Состояние «мёртв» |
| `Died` | Одноразовое событие смерти (снимается в конце кадра) |
| `DestroyOnDeath` | Реакция: удалить сущность при смерти (`DestroyOnDeathTrait`) |
| `PlayerDiedMessage` | MessagePipe-сообщение для слоя приложения |

`ActionBlockers { ActionBlockReason }` (`Gameplay/Actions`) — общий флаг «не может действовать». Причины: `Dead`. Его проверяют `DashStartSystem` и `AttackStartSystem`; при поднятом флаге прерываются даш (`DashEndSystem`) и атака (`AttackInterruptSystem`).

## Системы (группа Damage 800)

| Тип | Система | Что делает |
|---|---|---|
| Update | `DamageImmunitySystem` | Удаляет события урона по целям с ненулевым `DamageImmunity` |
| Update | `FactionDamageFilterSystem` | Удаляет события урона, если у источника и цели есть фракции и нет враждебности (`Factions.md`) |
| Update | `DamageApplySystem` | `Health.Current -= Amount` (не ниже 0). Мёртвых и сущности без `Health` пропускает |
| Update | `DeathSystem` | `Current <= 0` → `Dead` + `Died`, флаги `MovementBlockReason.Dead` и `ActionBlockReason.Dead` |
| Update | `DestroyOnDeathSystem` | `Died` + `DestroyOnDeath` → `DestroySelfRequest` |
| Update | `PlayerDeathNotifySystem` | `PlayerControlled` + `Died` → `PlayerDiedMessage` |
| Cleanup | `DamageEventCleanupSystem` | Удаляет сущности `DamageEvent` |
| Cleanup | `LateComponentCleanupSystem<Died>` | Снимает `Died` |

Смерть игрока: `GameplayEntry` получает `PlayerDiedMessage` и вызывает `ISceneSystem.ReloadGameplay()`.

## Точки расширения

- Модификаторы урона (броня, резисты, крит) — системы между `FactionDamageFilter` и `DamageApply`, которые меняют `Amount` или удаляют событие.
- Реакции на смерть — новые трейты по образцу `DestroyOnDeath`. План: `SpawnOnDeathTrait` для лутаемого объекта.
- Обратная связь при попадании (вспышка спрайта, полоска HP, отбрасывание) — реакции на `DamageEvent` до его очистки.
