# Integration tests

Integration tests verify the framework on the reference application `_SampleSystem` with a real SQL Server database. Each test calls the WebApi controllers directly through DI, without HTTP.

## Project structure

All projects are in `src/_SampleSystem/_Tests/`.

| Project | Purpose |
|---|---|
| `SampleSystem.IntegrationTests` | Abstract test classes (`EmployeeTests`, `SecurityRuleTests`, …) and the shared environment (`_Environment/`). Cannot be run: it has no entry point |
| `SampleSystem.IntegrationTests.EntityFramework` | Runs the same tests on Entity Framework. Each abstract class has an `*Impl` subclass |
| `SampleSystem.IntegrationTests.NHibernate` | Runs the same tests on NHibernate |
| `SampleSystem.DbGenerate` | Shared FluentMigrator migrations (`AddAncestorChildViews`, …) |
| `SampleSystem.DbGenerate.EntityFramework` / `.NHibernate` | Generation of an empty DB schema for a specific ORM |

The "abstract test + `*Impl`" pattern ensures both ORMs pass the same set of checks:

```csharp
// SampleSystem.IntegrationTests (shared)
public abstract class EmployeeTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void AddNewEmployee_CheckEmployeeSaved() { /* ... */ }
}

// SampleSystem.IntegrationTests.EntityFramework (and similarly in .NHibernate)
public class EmployeeTestsImpl(IServiceProvider rootServiceProvider) : EmployeeTests(rootServiceProvider);
```

To add a new test, create an abstract class in `SampleSystem.IntegrationTests` and a subclass in each of the two ORM projects.

## Requirements

- .NET SDK 10 (see `global.json`: runner `Microsoft.Testing.Platform`).
- SQL Server. CI uses the container `mcr.microsoft.com/mssql/server:2019-latest` (Express) with `sa` / `P@ssw0rd` on port 1433.

You can start the same container locally like this:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=P@ssw0rd" -e "MSSQL_PID=Express" -p 1433:1433 -d --name sqlexpress mcr.microsoft.com/mssql/server:2019-latest
```

## Running

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.EntityFramework
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.NHibernate
```

## Settings

Settings are read in `TestEnvironment` from the following sources. A later source overrides an earlier one:

1. `testAppSettings.json` next to the test assembly (required);
2. `<MachineName>.testAppSettings.json` (optional, for local overrides, do not commit it);
3. environment variables with the `SampleSystem_` prefix.

Example `testAppSettings.json`:

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

The `AutomationFrameworkSettings` section (`Framework.AutomationCore.AutomationFrameworkSettings`):

| Parameter | Default | Description |
|---|---|---|
| `IntegrationTestUserName` | `IntegrationTestRootUser` | The user on whose behalf tests run (receives the administrator role) |
| `DatabaseInitMode` | `RebuildSnapshot` | DB preparation mode (enum `Anch.Testing.Database.DatabaseInitMode`; with `RebuildSnapshot` the schema is recreated and a snapshot is saved) |
| `TestsParallelize` | `true` | Parallel test execution (each test gets a copy of the DB from the snapshot) |
| `DatabaseCollation` | empty | Collation of the created DB |
| `BackupPath` | empty | SQL Server directory for snapshots. On Linux usually `/var/opt/mssql/data` |
| `SecondaryDatabases` | `[]` | Additional databases |
| `LocalAdmins` | `[]` | Logins that are given the `Administrator` role during initialization |

The connection string must point to a server where the user has rights to create databases. In `Data Source=.` replace the dot with your instance if SQL Server is not local.

## How the environment works

The base class `Framework.AutomationCore.BssTestEnvironment` builds an `IServiceProvider` with all application services (`AddGeneralDependencyInjection`) and plugs in the DB testing infrastructure from the `Anch.Testing.*` packages:

1. **Empty schema.** `EmptySchemaInitializer` creates the DB via `DbGeneratorTest.GenerateAllDb` (FluentMigrator migrations) and runs the SQL scripts from `__Support/Scripts/SampleSystem`.
2. **Test data.** `TestDataInitializer` calls `SampleSystemInitializer`, assigns roles (`SetAdminRoleAsync`), creates an `Employee` for the test user and fills the reference data (`FillMainData`).
3. **Snapshot.** With `DatabaseInitMode = RebuildSnapshot` the result is saved as a DB snapshot (`BackupPath`), and test parallelism is controlled by `TestsParallelize`. Implementation details are in the `Anch.Testing.Database*` packages.
4. **Cleanup.** After the tests `BssCleanupTestEnvironmentHook` runs.

The assembly is wired up with the attribute `[assembly: AnchTestFramework<EntityFrameworkTestEnvironment>]`, and the NHibernate project uses an analogous class with the `NHibernate` extension.

The provider is built with `ValidateScopes` and `ValidateOnBuild`, so DI registration errors show up at test startup. `DuplicateServiceUsageValidator` also runs.

## Writing a test

Inherit from `TestBase` (`SampleSystem.IntegrationTests._Environment.TestData`). It provides:

- `MainWebApi`, `MainAuditWebApi`: wrappers over the controllers;
- `GetControllerEvaluator<T>(principalName)`: call any controller on behalf of a given user;
- `DataManager`: quick creation of test entities (`SaveEmployee`, `SaveCountry`, …);
- `AuthManager`: granting roles and permissions (`For(name).SetRoleAsync(...)`);
- `ActualConnectionString`: the connection string of the current test's DB, needed for direct SQL checks.

The `[AnchFact]` attribute (`Anch.Testing.Xunit`) is needed for async tests that require a `CancellationToken`. Otherwise a plain `[Fact]` is enough.

A test follows the Arrange / Act / Assert pattern:

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

The workflow `.github/workflows/integration-tests.yml` runs on push to `main`, on pull requests and manually (`workflow_dispatch`). It starts SQL Server in Docker, builds the solution and runs the EF and NHibernate tests sequentially.

## Common problems

| Symptom | Cause |
|---|---|
| DB connection error | SQL Server is not running or the connection string in `testAppSettings.json` is wrong |
| Snapshot / access rights error | Wrong `BackupPath`: the directory must be writable by the SQL Server process |
| Fails on only one ORM | The test exposed a difference in EF and NHibernate behavior, which is the purpose of having two sets |
| Failure on startup with a DI error | `ValidateOnBuild` or `DuplicateServiceUsageValidator` triggered: check the registrations |
