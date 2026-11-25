# CLAUDE.md

Основные правила проекта — в `AGENTS.md`. Документация — в `Docs/`. Прочитай их перед работой.

## Пользователь

- Пишет на Unity со стеком Morpeh + VContainer + MessagePipe + R3 + UniTask.
- Ждёт роли независимого эксперта: предлагать альтернативы и более сильные версии идей, думать архитектурно, проверять себя вопросом «не костыль ли это».
- **Никаких комментариев в коде.**
- Общение на русском.

## Работа через Unity MCP

- Перед работой убедись, что активен инстанс Unity этого проекта (`daggerfall@...`, `set_active_instance`), и проверь `Application.dataPath`.
- Проверку делай через `execute_code` (CodeDom, C# 6):
  - extension-методы Morpeh вызывай статически: `Scellecs.Morpeh.WorldStashExtensions.GetStash<T>(world)`, `WorldEntityExtensions.CreateEntity(world)`;
  - `Stash.Set` со значением вызывай с `ref`.
- Play Mode в фоне не тикает. Перед тестами выполни `Application.runInBackground = true; EditorApplication.QueuePlayerLoopUpdate();`.
- Сущность агента в Play Mode проще всего достать через `EntityInitializer` на его GameObject (`.World`, `.Entity`).
- Удаление ассетов делай через `AssetDatabase.DeleteAssets` (нужен `safety_checks: false`), чтобы ушли и `.meta`.
- Ассеты с `SerializeReference` надёжнее менять через `SerializedObject` + `managedReferenceValue = Activator.CreateInstance(type)`, чем руками в YAML.

## Завершение итерации

Обнови документы в `Docs/` и синхронизируй их с проектом DAGGERFALL на claude.ai. Основной источник — репозиторий.
