# Архитектура

## Слои

| Слой | Где | Что делает |
|---|---|---|
| Application | `Scripts/Game/Application` | DI-скоупы, загрузка сцен (Addressables), сохранения, GUI (окна, оверлеи, загрузка), обёртка над Input System |
| Gameplay (ECS) | `Scripts/Game/Gameplay` | Вся игровая логика на Morpeh: компоненты, трейты, системы, фичи |
| View | `Gameplay/View`, binds в фичах | Связь сущности с GameObject: `EntityInitializer`, `IEntityBind`, `AgentAuthoring` |
| UI | `Scripts/Game/Overlays`, `Scripts/Game/Windows` | Canvas-оверлеи и окна. Данные из ECS получают через MessagePipe |

## DI-скоупы (VContainer)

- **`SystemLifetimeScope`** — корневой, живёт всё время (DontDestroyOnLoad). Регистрирует: MessagePipe (+ `GlobalMessagePipe`), `IInputSystem`, `ISceneSystem`, `IAssetLoader` (Addressables), `IGUISystem`, `IWindowSystem`, `IOverlaySystem`, `ILoadingSystem`, сохранения, аудио, основную `Camera`, `ScenesConfig`.
- **`GameplayLifetimeScope`** — на сцене геймплея. Автоинъекция в `[INITIALIZATION]`, `[MANAGERS]`, `[ACTORS]`. Собственных регистраций пока нет.
- MessagePipe зарегистрирован открытыми generic'ами: `IPublisher<T>` / `ISubscriber<T>` резолвятся для любого `T` без отдельной регистрации.

## Поток запуска

1. Сцена `Bootstrap` → `IntroShower` → `ISceneSystem.LoadMainMenu()`.
2. `MainMenuWindow` → загрузка или создание сейва → `ISceneSystem.LoadGameplay(levelIndex)`. Сцены берутся из `ScenesConfig` по индексу уровня; уровень 0 — `TestGameplay`.
3. На сцене геймплея `GameplayEntry : SceneEntryPoint` в `Start` вызывает `Initialize`:
   - создаёт оверлеи сцены (`_usingOverlays`, через `IOverlaySystem`; создание идемпотентно);
   - `_world = World.Create(); _world.UpdateByUnity = true;`;
   - находит агентов, расставленных на сцене (`FindObjectsByType<AgentAuthoring>`);
   - регистрирует фичи в порядке `GameplayOrder`;
   - подписывается на `PlayerDiedMessage` и вызывает `ISceneSystem.ReloadGameplay()`.
4. `GameplayEntry.OnDestroy` → `_world.Dispose()`: системы освобождаются, подписки на ввод снимаются, view удаляются.

Если запустить `TestGameplay` прямо из редактора, сейва нет. Тогда `ReloadGameplay` грузит уровень 0 и ничего не сохраняет.

## Мир ECS и порядок групп

`GameplayOrder` задаёт порядок групп. Внутри фазы Unity (FixedUpdate / Update / LateUpdate / cleanup) группы выполняются по возрастанию order, системы внутри группы — в порядке добавления.

| Order | Группа | Fixed | Update | Late | Cleanup |
|---|---|---|---|---|---|
| 0 | Spawn | | инициализаторы: игрок, агенты сцены | | |
| 100 | View | | CreateView, ViewToPosition | Billboard | DestroyView |
| 200 | Input | | инициализаторы-подписчики на `InputEvents` | | |
| 210 | Perception | | Perception | | |
| 220 | Ai | | Context, Scoring, Selection, Wander/Chase executors, cleanup `AiActivated` | | AiActionOwnerDestroy |
| 230 | Navigation | | NavPath, NavFollow, NavIdle | | |
| 300 | VelocityReset | ResetVelocity | | | |
| 400 | Dash | Cooldown, Start, Velocity, End, cleanup `DashRequest` | | | |
| 500 | Movement | MovementAvailability, MoveDirectionToVelocity | CameraRelativeInput, CameraFacing, FaceMoveDirection | | |
| 600 | VelocityApply | VelocityToRigidbody | | | |
| 700 | Combat | | CameraAim, AttackInterrupt, Recovery, Swing, MeleeHit, Charge, Start, WeaponPresentation, cleanup `AttackHit`/`AttackRequest` | | WeaponOwnerDestroy |
| 800 | Damage | | Immunity, Apply, Death, DestroyOnDeath, PlayerDeathNotify | | DamageEventCleanup, cleanup `Died` |
| 10000 | Destroy | | | | DestroyEntities |

Ключевые инварианты:
- Конвейер скорости: Reset (300) → источники скорости (Dash 400, Movement 500) → Apply (600). Новый источник скорости (отбрасывание, рывок) — отдельная группа между 300 и 600.
- `DashStart` идёт до `MovementAvailability`, иначе в первый тик даша примешается обычное движение.
- Восприятие → ИИ → навигация идут в Update до Movement. `MoveDirection`, записанный в Update, читается в следующем fixed-тике.
- Удаление сущностей: `DestroySelfRequest` → DestroyView (100) → удаление детей (220, 700) → DestroyEntities (10000). Всё в cleanup-фазе того же кадра.

## Модель сущностей

```
Агент (игрок или враг)
├─ Position, View, [AssetPath]
├─ Movement:   MoveSpeed, Velocity, MoveDirection, FacingDirection, MovementBlockers, RigidbodyComponent (bind)
├─ Control:    PlayerControlled + InputMoveDirection  |  AiBrain + AiContext
├─ Dash:       DashParameters, [Dashing], [DashCooldown], [DashRequest]
├─ Combat:     EquippedWeapon → (оружие), AttackIntent, AimRay, ActionBlockers, [AttackRequest]
├─ Damage:     Health, DamageImmunity, [Dead], [Died], [DestroyOnDeath]
├─ Perception: PerceptionParameters, PerceivedTarget
└─ Navigation: NavAgent, NavPath, [NavDestination], [NavArrived]

Оружие (отдельная сущность)
├─ WeaponOwner → агент
├─ AttackTiming, WeaponDamage, [ChargeAttack], [MeleeHit], WeaponPresentationRef
└─ фазы: [AttackCharging] | [AttackSwing] | [AttackRecovery], одноразовый [AttackHit]

Действие ИИ (отдельная сущность на каждое действие агента)
├─ AiAction { Owner, Weight, Considerations, Score }
├─ WanderAction | ChaseAction | ...
└─ [AiActive], одноразовый [AiActivated]

DamageEvent (сущность-событие, живёт один кадр)
```

## Конфиги и создание сущностей

- `EntityConfig<TTrait> : ScriptableObject` — список `[SerializeReference] TTrait[]` с валидацией TriInspector (пустые слоты, дубли) и методом `Install(entity, world)`.
  - `AgentConfig : EntityConfig<IAgentTrait>`
  - `WeaponConfig : EntityConfig<IWeaponTrait>` + id, отображаемое имя, `WeaponType`.
- `world.CreateAgent(config, position)` (`AgentFactory`) создаёт сущность, ставит `Position` и устанавливает трейты.
- Игрок создаётся из `GameplayEntry._playerConfig` (`AgentSpawnInitializer`, пока в `Vector3.zero`).
- Агенты, расставленные на сцене, — это GameObject с `AgentAuthoring` (ссылка на `AgentConfig`) и `EntityInitializer`. `SceneAgentSpawnInitializer` создаёт им сущности и привязывает существующий GameObject как view.
- Агенты из префабов: `ViewTrait` кладёт `AssetPath`, `CreateViewSystem` инстанцирует префаб через Addressables.

## Мост ECS → UI

- `WeaponPresentationSystem` публикует каждый кадр `WeaponPresentationMessage` (фаза, прогресс, заряд, `WeaponPresentation`). Его слушает `WeaponOverlay` на Canvas.
- `PlayerDeathNotifySystem` публикует `PlayerDiedMessage`. Его слушает `GameplayEntry`, позже — экран смерти.
- Оверлеи живут в корневом скоупе на Canvas с DontDestroyOnLoad. `OverlaySystem` создаёт оверлей одного типа только один раз. Полноценная привязка жизни оверлея к сцене — в роадмапе.

## Структура папок

```
Assets/_Project/
  Configs/Gameplay/        AgentConfig игрока и врагов, Weapons/ (WeaponConfig, WeaponPresentation)
  Configs/System/          ScenesConfig
  Prefabs/Gameplay/Actors/ PlayerAvatar (Rigidbody, капсула, CinemachineCamera на оси капсулы)
  Prefabs/GUI/Overlays/    оверлеи (Addressables-ключ = имя OverlayType)
  Scenes/                  Bootstrap, MainMenu, TestGameplay (NavMeshSurface, TestEnemy)
  Scripts/Game/Application
  Scripts/Game/Gameplay/<Feature>
  Scripts/Game/Overlays, Windows
Docs/
```
