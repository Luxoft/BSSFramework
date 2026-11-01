# Contributing

Thank you for your interest in BSS Framework. This document describes how to build the project, run the tests and prepare changes. For an overview of the repository structure, see [docs/architecture.md](docs/architecture.md).

## Requirements

- .NET SDK 10 (`global.json`: test runner `Microsoft.Testing.Platform`).
- Windows, Linux or macOS. CI runs on Ubuntu.
- SQL Server for integration tests and DB schema generation ([docs/integrationtests.md](docs/integrationtests.md)).

## Build

Main solution: `src/BssFramework.slnx`. Secondary: `src.other/Framework.Other.sln` (Graphics, Installer, Localization, Transliteration, scripts).

```bash
dotnet build src/BssFramework.slnx -c Release
```

The solution is large (~140 projects). During development build the specific project you changed, and do a full build before submitting changes. Package versions are managed centrally in `src/Directory.Packages.props`. Do not specify versions in `.csproj` files.

## Tests

As in CI (`.github/workflows/build+unit.yml`):

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.CheckGenTests
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.UnitTests
dotnet test --project src/Framework.ExpressionParsers.Tests
dotnet test --project src/Validation/Framework.Validation.Tests
dotnet test --project src/Application/Framework.Application.Tests
dotnet test --project src/Notification/Framework.Notification.Tests
dotnet test --project src/Database/Framework.Database.NHibernate.DBGenerator.Tests
```

Integration tests (require SQL Server, workflow `integration-tests.yml`):

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.EntityFramework
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.NHibernate
```

`Framework.Tracking.Tests` exists, but CI does not currently run it. Run it manually if you change `Framework.Tracking`.

## Generated code

Many files in the repository are generated: `*.Generated.cs`, `Generated.*` projects, `*.hbm.xml`. **Do not edit them by hand.** Change the domain model or the generator, then run generation and commit the result together with the change.

The CI check `SampleSystem.CheckGenTests/GenerationTests` fails if generation produces new or modified files. Details and commands: [docs/codegeneration.md](docs/codegeneration.md).

## Conventions

### Code

- **Nullable is enabled everywhere, with 0 warnings.** Do not add new ones. The authoritative check is a full solution build with `--no-incremental` (per-project counts are unreliable).
  - In domain assemblies, models and attributes do not use `required`. Initialize mandatory fields (Master references, `[Required]`, `.Not.Nullable()` in FluentNHibernate) with `= null!`, and declare optional ones as `Type?`.
  - Mark reflection results (`GetMethod`, `GetProperty`, `ReflectedType`) with `!`.
  - For generic keys and values that are never `null`, use `where T : notnull` / `class`.
  - For types with an overloaded `operator ==`, compare with `null` via `is null`.
  - `GetById(id, throwOnNotFound: true)!`.
  - **Do not change runtime behavior just to silence the compiler.** Use `value!`, not `?? fallback`.
- Style: follow the surrounding code (naming, comment density, idioms). Files are saved with a BOM, do not strip it.
- `CS4014`, `CS1998`, `CA2016`, `xUnit1051`, `xUnit1031` are treated as errors in the build (`WarningsAsErrors` in `Directory.Build.props`).
- XML documentation (`GenerateDocumentationFile=true`): warning CS1591 is suppressed globally. Still, document public APIs, especially in `*.Abstractions`.

### Projects

- New projects: `PackageId` = `Luxoft.<ProjectName>` (or omit it). This is checked by `CsprojValidationTests`.
- Domains are split into `*.Abstractions` (contracts) / `*.Domain` (models) / `*.Runtime` (implementation) / `*.Tests`.
- Extension points are wired through DI (`ServiceCollectionExtensions`, `Framework.DependencyInjection`) and contracts in `*.Abstractions`.
- `*.Legacy*` is a deprecated API: do not use it in new code ([docs/legacy.md](docs/legacy.md)).

### Tests

- Write an integration test as an abstract class in `SampleSystem.IntegrationTests` plus an `*Impl` subclass in each of the EF and NHibernate projects.
- Arrange / Act / Assert structure.
- Method names: `Method_Condition_Result` (as in existing tests, e.g. `AddNewEmployee_CheckEmployeeSaved`).

## Process

1. Create a branch from `main`.
2. Make your changes. If you changed the model or the generators, run generation (all three systems: `_Authorization`, `_Configuration`, `_SampleSystem`).
3. Build the solution and run the tests (see above).
4. Open a pull request to `main`. The `Build+Unit` and `Integration tests` workflows run on PRs.
5. Commit and PR messages: short and to the point.

## Publishing packages

Packages are published to NuGet when a GitHub Release is created (`publish.yml`). The package version is taken from the release tag name (`/p:PackageVersion=<tag>`), and `dotnet pack` packs all packages of the solution.

## Documentation

Documentation lives in `docs/` (see the table of contents in [readme.md](readme.md)). When you change the behavior of a subsystem, update the corresponding document. Examples in the documents are taken from `_SampleSystem`; if you change the source of an example, update the document too.
