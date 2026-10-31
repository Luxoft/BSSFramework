# Интеграционные тесты

Интеграционные тесты проверяют фреймворк на референсном приложении `_SampleSystem` с реальной базой SQL Server. Каждый тест вызывает WebApi-контроллеры напрямую через DI, без HTTP.

## Структура проектов

Все проекты лежат в `src/_SampleSystem/_Tests/`.

| Проект | Назначение |
|---|---|
| `SampleSystem.IntegrationTests` | Абстрактные тестовые классы (`EmployeeTests`, `SecurityRuleTests`, …) и общее окружение (`_Environment/`). Запускать нельзя: тут нет точки входа |
| `SampleSystem.IntegrationTests.EntityFramework` | Запуск тех же тестов на Entity Framework. Для каждого абстрактного класса есть наследник `*Impl` |
| `SampleSystem.IntegrationTests.NHibernate` | Запуск тех же тестов на NHibernate |
| `SampleSystem.DbGenerate` | Общие миграции FluentMigrator (`AddAncestorChildViews`, …) |
| `SampleSystem.DbGenerate.EntityFramework` / `.NHibernate` | Генерация пустой схемы БД для конкретного ORM |

Паттерн «абстрактный тест + `*Impl`» гарантирует, что оба ORM проходят один и тот же набор проверок:

```csharp
// SampleSystem.IntegrationTests (общий)
public abstract class EmployeeTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void AddNewEmployee_CheckEmployeeSaved() { /* ... */ }
}

// SampleSystem.IntegrationTests.EntityFramework (и аналогично в .NHibernate)
public class EmployeeTestsImpl(IServiceProvider rootServiceProvider) : EmployeeTests(rootServiceProvider);
```

Чтобы добавить новый тест, создайте абстрактный класс в `SampleSystem.IntegrationTests` и по наследнику в каждом из двух ORM-проектов.

## Требования

- .NET SDK 10 (см. `global.json`: раннер `Microsoft.Testing.Platform`).
- SQL Server. В CI используется контейнер `mcr.microsoft.com/mssql/server:2019-latest` (Express) с `sa` / `P@ssw0rd` на порту 1433.

Локально поднять такой же контейнер можно так:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@ssw0rd" -e "MSSQL_PID=Express" -p 1433:1433 -d --name sqlexpress mcr.microsoft.com/mssql/server:2019-latest
```

## Запуск

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.EntityFramework
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.NHibernate
```

## Настройки

Настройки читаются в `TestEnvironment` из следующих источников. Более поздний источник перекрывает предыдущий:

1. `testAppSettings.json` рядом с тестовой сборкой (обязателен);
2. `<MachineName>.testAppSettings.json` (необязателен, для локальных переопределений, не коммитьте его);
3. переменные окружения с префиксом `SampleSystem_`.

Пример `testAppSettings.json`:

```json
{
  "AutomationFrameworkSettings": {
    "DatabaseInitMode": "RebuildSnapshot",
    "BackupPath": "/var/opt/mssql/data",
    "TestsParallelize": true
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=SampleSystem;User Id=sa;Password=P@ssw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True"
  }
}
```

Секция `AutomationFrameworkSettings` (`Framework.AutomationCore.AutomationFrameworkSettings`):

| Параметр | По умолчанию | Описание |
|---|---|---|
| `IntegrationTestUserName` | `IntegrationTestRootUser` | Пользователь, от имени которого выполняются тесты (получает роль администратора) |
| `DatabaseInitMode` | `RebuildSnapshot` | Режим подготовки БД (enum `Anch.Testing.Database.DatabaseInitMode`; при `RebuildSnapshot` схема пересоздаётся и сохраняется снимок) |
| `TestsParallelize` | `true` | Параллельный запуск тестов (каждый получает копию БД из снимка) |
| `DatabaseCollation` | пусто | Collation создаваемой БД |
| `BackupPath` | пусто | Каталог SQL Server для снимков. На Linux обычно `/var/opt/mssql/data` |
| `SecondaryDatabases` | `[]` | Дополнительные БД |
| `LocalAdmins` | `[]` | Логины, которым при инициализации выдаётся роль `Administrator` |

Строка подключения должна указывать на сервер, где у пользователя есть права создавать БД. В `Data Source=.` вместо точки подставьте свой экземпляр, если SQL Server не локальный.

## Как устроено окружение

Базовый класс `Framework.AutomationCore.BssTestEnvironment` строит `IServiceProvider` со всеми сервисами приложения (`AddGeneralDependencyInjection`) и подключает инфраструктуру тестирования БД из пакета `Anch.Testing.*`:

1. **Пустая схема.** `EmptySchemaInitializer` создаёт БД через `DbGeneratorTest.GenerateAllDb` (миграции FluentMigrator) и выполняет SQL-скрипты из `__Support/Scripts/SampleSystem`.
2. **Тестовые данные.** `TestDataInitializer` вызывает `SampleSystemInitializer`, назначает роли (`SetAdminRoleAsync`), создаёт `Employee` для пользователя тестов и заполняет справочники (`FillMainData`).
3. **Снимок.** При `DatabaseInitMode = RebuildSnapshot` результат сохраняется как снимок БД (`BackupPath`), а параллельность тестов регулируется `TestsParallelize`. Детали реализации находятся в пакетах `Anch.Testing.Database*`.
4. **Очистка.** После тестов срабатывает `BssCleanupTestEnvironmentHook`.

Сборка подключается атрибутом `[assembly: AnchTestFramework<EntityFrameworkTestEnvironment>]`, а в NHibernate-проекте аналогичным классом с `NHibernate`-расширением.

Провайдер собирается с `ValidateScopes` и `ValidateOnBuild`, поэтому ошибки регистрации в DI проявляются на старте тестов. Дополнительно работает `DuplicateServiceUsageValidator`.

## Написание теста

Наследуйтесь от `TestBase` (`SampleSystem.IntegrationTests._Environment.TestData`). Он даёт:

- `MainWebApi`, `MainAuditWebApi`: обёртки над контроллерами;
- `GetControllerEvaluator<T>(principalName)`: вызов произвольного контроллера от имени заданного пользователя;
- `DataManager`: быстрое создание тестовых сущностей (`SaveEmployee`, `SaveCountry`, …);
- `AuthManager`: выдача ролей и прав (`For(name).SetRoleAsync(...)`);
- `ActualConnectionString`: строка подключения к БД текущего теста, нужна для прямых SQL-проверок.

Атрибут `[AnchFact]` (`Anch.Testing.Xunit`) нужен для асинхронных тестов, которым необходим `CancellationToken`. В остальных случаях подойдёт обычный `[Fact]`.

Тест строится по схеме Arrange / Act / Assert:

```csharp
[Fact]
public void AddNewEmployee_CheckEmployeeSaved()
{
    // Arrange
    var employeeController = this.MainWebApi.Employee;
    var employeeIdentity = this.DataManager.SaveEmployee(Guid.NewGuid());

    // Act
    var employees = employeeController.Evaluate(c => c.GetSimpleEmployees());

    // Assert
    Assert.Contains(employees, e => e.Id == employeeIdentity.Id);
}
```

## CI

Workflow `.github/workflows/integration-tests.yml` запускается на push в `main`, на pull request и вручную (`workflow_dispatch`). Он поднимает SQL Server в Docker, собирает решение и последовательно запускает EF- и NHibernate-тесты.

## Частые проблемы

| Симптом | Причина |
|---|---|
| Ошибка подключения к БД | SQL Server не запущен или неверна строка подключения в `testAppSettings.json` |
| Ошибка на снимках / прав доступа | Неверный `BackupPath`: каталог должен быть доступен процессу SQL Server на запись |
| Падают только на одном ORM | Тест вскрыл различие в поведении EF и NHibernate, это и есть цель двух наборов |
| Падение на старте с ошибкой DI | Сработал `ValidateOnBuild` или `DuplicateServiceUsageValidator`: проверьте регистрации |
