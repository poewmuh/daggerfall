# Daggerfall — правила для агентов и разработчиков

3D-игра от первого лица в духе The Elder Scrolls II: Daggerfall. Спрайтовые враги (billboard), ближний бой с зарядом, даш, Utility AI.

Перед работой прочитай:
- `Docs/Architecture.md` — слои, мир ECS, порядок систем, модель сущностей.
- `Docs/Features/<Фича>.md` — документ фичи, которую трогаешь.
- `Docs/Decisions.md` — почему сделано именно так. Не «улучшай» обратно то, что там обосновано.
- `Docs/Roadmap.md` — что запланировано и какие есть известные проблемы.

## Стек

| Что | Версия |
|---|---|
| Unity | 6000.3, URP 17.3 |
| ECS | Morpeh 2024.1.1 (`Scellecs.Morpeh`) |
| DI | VContainer 1.17 |
| События | MessagePipe 1.8 (с интеграцией VContainer) |
| Реактивность | R3 1.3 |
| Асинхронность | UniTask 2.5 |
| Инспектор | TriInspector 1.15 |
| Камера | Cinemachine 3.1 |
| Навигация | AI Navigation 2.0 (NavMeshSurface + запросы NavMesh) |
| Ассеты и сцены | Addressables 2.9 |
| Ввод | Input System 1.18 |

## Жёсткие правила

1. **Никаких комментариев в коде.** Код должен читаться без них.
2. **Вся игровая логика — в системах Morpeh.** MonoBehaviour используются только для слоя приложения (DI, сцены, UI) и view (binds, `EntityInitializer`, `AgentAuthoring`).
3. **Никогда не используй `World.Default`.** Мир создаёт и уничтожает `GameplayEntry` (`World.Create` / `Dispose`).
4. **Состояние — это наличие компонента.** `Dashing`, `AttackSwing`, `DashCooldown`, `Dead`. Не заводи bool-флаги и enum-машины состояний внутри одного компонента.
5. **Блокировки — через флаги причин:** `MovementBlockers`, `ActionBlockers`, `DamageImmunity`. Новая причина — новый флаг enum. Не расставляй `Without<X>` по разным системам.
6. **Одноразовые запросы** (`DashRequest`, `AttackRequest`, `AttackHit`, `AiActivated`, `Died`) снимаются системой очистки того же типа обновления, что и потребитель:
   - `ComponentCleanupSystem<T>` — в Update;
   - `FixedComponentCleanupSystem<T>` — в FixedUpdate;
   - `LateComponentCleanupSystem<T>` — в конце кадра (cleanup-фаза).

   Ошибка в выборе даёт потерянные запросы, если в кадре нет fixed-тика.
7. **События, которых за кадр может быть несколько, — отдельные сущности** (`DamageEvent`). В Morpeh нет буферов на сущности.
8. **Трейт или bind:** значение из ассета → трейт (`IAgentTrait` / `IWeaponTrait`); ссылка на объект сцены или префаба → `IEntityBind`.
9. **Движение физических тел** — только через `MoveDirection` / `Velocity`. В Rigidbody пишет только `VelocityToRigidbodySystem`. `NavMeshAgent` не используем.
10. **ECS → UI** — через presenter-системы и MessagePipe (`WeaponPresentationSystem` → `WeaponPresentationMessage`). UI не читает `World`.
11. **Сущности-«дети»** (оружие, действия ИИ) удаляются вместе с владельцем через cleanup-систему по образцу `WeaponOwnerDestroySystem`.
12. **Тип системы:**
    - физика и всё, что влияет на скорость (даш, конвейер скорости) — `IFixedSystem`;
    - логика, ИИ, бой, урон — `ISystem`;
    - то, что должно идти после камеры (billboard) — `ILateSystem`;
    - уборка в конце кадра — `ICleanupSystem`.

## Как устроена фича

```
Gameplay/<Feature>/
  <Feature>Components.cs      структуры IComponent
  <Feature>Trait.cs           [Serializable] класс : IAgentTrait (или IWeaponTrait), Install(entity, world)
  Systems/<Name>System.cs     sealed, интерфейсы реализованы явно
  <Feature>Feature.cs         static SystemsGroup Create(World world, ...зависимости)
```

Подключение — в `GameplayEntry.Initialize`: `_world.AddSystemsGroup(GameplayOrder.<Feature>, <Feature>Feature.Create(...))`. Новый порядок добавляется константой в `GameplayOrder`.

Шаблон системы:

```csharp
public sealed class ExampleSystem : ISystem
{
	private Filter _filter;
	private Stash<Example> _exampleStash;

	public World World { get; set; }

	void IInitializer.OnAwake()
	{
		_filter = World.Filter.With<Example>().Build();
		_exampleStash = World.GetStash<Example>();
	}

	void ISystem.OnUpdate(float deltaTime)
	{
		foreach (var entity in _filter)
		{
			ref var example = ref _exampleStash.Get(entity);
		}
	}

	void IDisposable.Dispose() { }
}
```

Зависимости (камера, publisher, сервисы приложения) передаются в конструктор системы из `Feature.Create`, а туда — из `GameplayEntry` (`[Inject]`).

В трейтах используй `Stash.Set` (идемпотентно). Для компонентов, которые нужны нескольким трейтам (`ActionBlockers`), — `stash.EnsureComponent(entity)` из `GameplayStashExtensions`.

## Именование

- Namespace совпадает с папкой: `Game.Gameplay.<Feature>`, `.Systems`, `.Traits`.
- Компонент не должен называться так же, как namespace фичи: `PerceptionParameters`, а не `Perception`. Иначе из соседних namespace имя разрешится в namespace.
- Отступы — табы. Сохраняй переводы строк существующего файла: ассеты и сцены Unity — CRLF.

## Рабочий процесс

1. Прочитай документ фичи и `Decisions.md`.
2. Внося изменения, учитывай, что IDE может держать открытые файлы в буфере и записать их поверх. Был случай с `GameplayEntry.cs`.
3. Скомпилируй, проверь консоль, проверь поведение в Play Mode.
4. В конце итерации обнови `Docs/Features/<Фича>.md`, `Docs/Decisions.md` (если было решение) и `Docs/Roadmap.md`.
