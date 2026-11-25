# ИИ: восприятие, навигация, Utility AI

ИИ — это ещё один источник намерений, как ввод игрока. Решение выставляет `NavDestination` (позже `AttackRequest`, `AimRay`), а исполнение идёт через общие системы.

## Восприятие (`Gameplay/Perception`, группа 210, Update)

`PerceptionTrait` → `PerceptionParameters`, `PerceivedTarget`. Параметры по умолчанию:
- дальность 12;
- радиус «чутья» 1.5 (видит даже со спины);
- угол обзора 120°;
- высота глаз 0.6, высота точки цели 1.0;
- память 5 с;
- интервал опроса 0.2 с (стартовая фаза случайная, чтобы враги опрашивали в разное время);
- маска препятствий — всё, кроме `Ignore Raycast` и `Player`.

`PerceptionSystem`:
- каждый кадр обновляет память: пока цель не видна, растёт `TimeSinceSeen`; после `MemoryDuration` → `HasTarget = false`;
- с интервалом ищет ближайшую видимую цель: дальность, затем угол (или ближе радиуса чутья), затем `Physics.Linecast` от глаз до цели;
- цели пока — сущности с `PlayerControlled` без `Dead`. После фракций — враждебные.

`PerceivedTarget { Target, LastKnownPosition, TimeSinceSeen, Visible, HasTarget }`.

## Навигация (`Gameplay/Navigation`, группа 230, Update)

`NavAgentTrait` → `NavAgent { RepathInterval 0.25, CornerReachDistance 0.25, SampleDistance 2 }`, `NavPath { Corners[32] }`.

| Система | Что делает |
|---|---|
| `NavPathSystem` | При смене цели (> 0.3 м) или по таймеру: `SamplePosition` начала и конца, `NavMesh.CalculatePath`, углы пути в `NavPath` |
| `NavFollowSystem` | Идёт по углам и пишет плоский `MoveDirection`. В пределах `StoppingDistance` → `MoveDirection = 0` + `NavArrived` |
| `NavIdleSystem` | Без `NavDestination`: `MoveDirection = 0`, путь сброшен, `NavArrived` снят |

`NavDestination { Value, StoppingDistance }` ставят исполнители действий. `NavMeshAgent` не используется: он двигал бы тело в обход конвейера скорости. NavMesh запекается `NavMeshSurface` на сцене.

## Utility AI (`Gameplay/AI`, группа 220, Update)

### Модель

- `AiBrainTrait` → `AiBrain { ActiveAction, HasActiveAction, Inertia 0.1, BestAction, BestScore }` и `AiContext`.
- `AiContext` — «доска» входов для соображений: `HasTarget`, `TargetVisible`, `DistanceToTarget`, `PerceptionRange`, `TimeSinceSeen`, `MemoryDuration`, `HealthRatio`.
- Каждое действие — **отдельная сущность**: `AiAction { Owner, Weight, Considerations, Score }` плюс компонент конкретного действия. Его создаёт трейт-наследник `AiActionTrait`.
- Соображения (`IConsideration`, `[SerializeReference]`, настраиваются в инспекторе): `ConstantConsideration`, `HasTargetConsideration`, `TargetVisibleConsideration`, `DistanceToTargetConsideration` (кривая), `TimeSinceSeenConsideration` (кривая), `HealthConsideration` (кривая).
- `Score = Weight × Π clamp01(consideration)`.

### Системы

1. `AiContextSystem` — заполняет `AiContext` из восприятия и здоровья.
2. `AiScoringSystem` — считает `Score` всех действий. У мёртвого владельца — 0.
3. `AiSelectionSystem` — максимум по владельцу. Активное действие получает бонус `Inertia`. При смене действия: `AiActive` / `AiActivated` переходят к новому действию, `NavDestination` владельца снимается.
4. Исполнители (фильтр `AiActive`): `WanderExecutorSystem`, `ChaseExecutorSystem`.
5. `ComponentCleanupSystem<AiActivated>`.
6. `AiActionOwnerDestroySystem` (cleanup) — удаляет действия вместе с владельцем.

### Действия

**Wander** (`WanderTrait`): радиус 5 от точки спавна (`Home`), пауза 1–3 с, максимум 8 с на дорогу, остановка 0.3. Соображения по умолчанию: `Constant(0.2)` — базовое поведение.

**Chase** (`ChaseTrait`): остановка 1.2 м у видимой цели, 0.5 м у последней известной позиции. Если цель стоит рядом — разворачивает `FacingDirection` к ней. Соображения по умолчанию: `HasTarget × TimeSinceSeen` (кривая 1→0). Когда цель пропадает из виду, оценка затухает: враг идёт к последней известной позиции и через ~4.5 с уступает Wander. Отдельное состояние «поиск» не нужно.

### Как добавить действие

1. Компонент действия (`XAction`) + трейт `XTrait : AiActionTrait` с `CreateDefaultConsiderations()` и `InstallAction(...)`.
2. `XExecutorSystem`: фильтр `AiAction + AiActive + XAction`, обработка `AiActivated` как входа в действие. Пишет только намерения (`NavDestination`, `AttackRequest`, ...).
3. Добавить исполнитель в `AiFeature` после `AiSelectionSystem`.
4. Если нужен новый вход — поле в `AiContext`, заполнение в `AiContextSystem` и новое соображение.

## Конфиг тестового врага

`TestEnemyConfig`: `HealthTrait` 30, `MovementTrait` 3.5 (медленнее игрока — 5), `PerceptionTrait`, `NavAgentTrait`, `AiBrainTrait`, `WanderTrait`, `ChaseTrait`, `DestroyOnDeathTrait`.
