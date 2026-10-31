# Contributing

Спасибо за интерес к BSS Framework. Этот документ описывает, как собрать проект, прогнать тесты и оформить изменения. Обзор устройства репозитория: [docs/architecture.md](docs/architecture.md).

## Требования

- .NET SDK 10 (`global.json`: тестовый раннер `Microsoft.Testing.Platform`).
- Windows, Linux или macOS. CI работает на Ubuntu.
- SQL Server для интеграционных тестов и генерации схемы БД ([docs/integrationtests.md](docs/integrationtests.md)).

## Сборка

Основное решение: `src/BssFramework.slnx`. Дополнительное: `src.other/Framework.Other.sln` (Graphics, Installer, Localization, Transliteration, скрипты).

```bash
dotnet build src/BssFramework.slnx -c Release
```

Решение большое (~140 проектов). При разработке собирайте конкретный проект, а полную сборку делайте перед отправкой изменений. Версии пакетов задаются централизованно в `src/Directory.Packages.props`. В `.csproj` версии не указываются.

## Тесты

Как в CI (`.github/workflows/build+unit.yml`):

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.CheckGenTests
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.UnitTests
dotnet test --project src/Framework.ExpressionParsers.Tests
dotnet test --project src/Validation/Framework.Validation.Tests
dotnet test --project src/Application/Framework.Application.Tests
dotnet test --project src/Notification/Framework.Notification.Tests
dotnet test --project src/Database/Framework.Database.NHibernate.DBGenerator.Tests
```

Интеграционные тесты (нужен SQL Server, workflow `integration-tests.yml`):

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.EntityFramework
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.NHibernate
```

В `Framework.Tracking.Tests` тесты есть, но CI их сейчас не запускает. Запускайте вручную, если меняете `Framework.Tracking`.

## Сгенерированный код

Много файлов в репозитории сгенерировано: `*.Generated.cs`, проекты `Generated.*`, `*.hbm.xml`. **Руками их не правят.** Меняйте доменную модель или генератор, затем запускайте генерацию и коммитьте результат вместе с изменением.

CI-проверка `SampleSystem.CheckGenTests/GenerationTests` падает, если генерация даёт новые или изменённые файлы. Подробности и команды: [docs/codegeneration.md](docs/codegeneration.md).

## Соглашения

### Код

- **Nullable включён везде, предупреждений 0.** Не добавляйте новые. Авторитетная проверка: полная сборка решения с `--no-incremental` (по отдельным проектам счётчик ненадёжен).
  - В доменных сборках, моделях и атрибутах не используйте `required`. Обязательные поля (ссылки на Master, `[Required]`, `.Not.Nullable()` в FluentNHibernate) инициализируйте как `= null!`, необязательные объявляйте как `Type?`.
  - Результаты рефлексии (`GetMethod`, `GetProperty`, `ReflectedType`) помечайте `!`.
  - Для обобщённых ключей и значений, которые не бывают `null`, используйте `where T : notnull` / `class`.
  - Для типов с перегруженным `operator ==` сравнивайте с `null` через `is null`.
  - `GetById(id, throwOnNotFound: true)!`.
  - **Не меняйте поведение рантайма ради тишины компилятора.** Используйте `value!`, а не `?? fallback`.
- Стиль: следуйте окружающему коду (именование, плотность комментариев, идиомы). Файлы сохраняются с BOM, не удаляйте его.
- Ошибки `CS4014`, `CS1998`, `CA2016`, `xUnit1051`, `xUnit1031` в сборке считаются ошибками (`WarningsAsErrors` в `Directory.Build.props`).
- XML-документация (`GenerateDocumentationFile=true`): предупреждение CS1591 глушится глобально. Тем не менее документируйте публичные API, особенно в `*.Abstractions`.

### Проекты

- Новые проекты: `PackageId` = `Luxoft.<ИмяПроекта>` (или не указывайте его). Это проверяет `CsprojValidationTests`.
- Домены делятся на `*.Abstractions` (контракты) / `*.Domain` (модели) / `*.Runtime` (реализация) / `*.Tests`.
- Точки расширения подключаются через DI (`ServiceCollectionExtensions`, `Framework.DependencyInjection`) и контракты в `*.Abstractions`.
- `*.Legacy*` — устаревший API: в новом коде не используйте ([docs/legacy.md](docs/legacy.md)).

### Тесты

- Интеграционный тест пишите как абстрактный класс в `SampleSystem.IntegrationTests` и по наследнику `*Impl` в EF- и NHibernate-проектах.
- Структура Arrange / Act / Assert.
- Названия методов: `Метод_Условие_Результат` (как в существующих тестах, например `AddNewEmployee_CheckEmployeeSaved`).

## Процесс

1. Создайте ветку от `main`.
2. Внесите изменения. Если меняли модель или генераторы, выполните генерацию (все три системы: `_Authorization`, `_Configuration`, `_SampleSystem`).
3. Соберите решение и прогоните тесты (см. выше).
4. Откройте pull request в `main`. На PR запускаются workflows `Build+Unit` и `Integration tests`.
5. Сообщения коммитов и PR: коротко, по сути.

## Публикация пакетов

Пакеты публикуются в NuGet при создании GitHub Release (`publish.yml`). Версия пакетов берётся из имени тега релиза (`/p:PackageVersion=<tag>`), `dotnet pack` упаковывает все пакеты решения.

## Документация

Документация лежит в `docs/` (см. оглавление в [readme.md](readme.md)). При изменении поведения подсистемы обновляйте соответствующий документ. Примеры в документах взяты из `_SampleSystem`; если вы меняете пример-источник, поправьте и документ.
