# AGENTS.md — BSS Framework

Guide for AI agents working in this repository. Read before making changes.

## Overview

BSS Framework (Luxoft) is a .NET platform for rapid business-application development: Role&Context Base Security, Column Base Restrictions, notifications, subscriptions, validation, data audit (Envers/Inline), Hangfire/CAP, and **code generation** (database, DTO, ORM mapping, BLL, WebApi, TypeScript via gengen). Two ORMs are supported: **NHibernate** and **Entity Framework**.

- Repository: https://github.com/Luxoft/BSSFramework; NuGet packages are named `Luxoft.<ProjectName>`.
- Stack: **.NET 10** (`net10.0`), C# `latest`, `Nullable=enable`, `ImplicitUsings=enable`, xUnit on Microsoft.Testing.Platform (`global.json`).
- Package versions are managed centrally in `src/Directory.Packages.props` (`ManagePackageVersionsCentrally`). Never specify versions in `.csproj` files.
- Shared build properties live in `src/Directory.Build.props` (`GenerateDocumentationFile`, `WarningsAsErrors`: CS4014, CS1998, CA2016).

## Repository layout

Main solution: `src/BssFramework.slnx`. Secondary solution: `src.other/Framework.Other.sln` (Graphics, Installer, Localization, Transliteration, scripts).

Code lives in `src/`, grouped **by domain**. Within a domain the usual split is: `*.Abstractions` (contracts) / `*.Domain` (models) / `*.Runtime` (implementation) / `*.Tests`.

| Folder | Purpose |
|---|---|
| `Framework.Core` | Base layer (layer 0): extensions, reflection, expression utilities, Maybe/TryResult, Period/Range, serialization. Depends on nothing |
| `Framework.DependencyInjection`, `Framework.ExpressionParsers(.Tests)` | DI helpers, expression parsers |
| `Domain` (`Framework.Relations`, `Framework.Restriction`) | Attributes/metadata for the domain model |
| `Validation` | Validators, `IValidationData`, `IPropertyValidator` |
| `Database` | DB abstractions, `*.NHibernate`, `*.EntityFramework`, audit (`EnversAudit`, `InlineAudit`), `DBGenerator` (migrations), `DALGenerator`, `Tracking`, `SqlMapper`. `*.Legacy` sub-projects are the old API |
| `BLL` | BLL layer: `BLL.Abstractions/Domain/Runtime/Events/Validation`, `BLL.DTOMapping` (DTO mapping) |
| `Application` | Application layer (jobs, application services) |
| `Infrastructure` | Hangfire, Swagger, other infrastructure |
| `Notification`, `Subscriptions` | Notifications and subscriptions (Domain/Abstractions/Runtime/Metadata) |
| `Projection`, `ExtendedMetadata` | Projections and extended metadata |
| `Configurator` | Configurator (ApplicationVariable, Events) |
| `AutomationCore` | Integration-test infrastructure (`Anch.Testing.Database.Mssql`, etc.) |
| `CodeGeneration` | **Code generators**: `CodeDom`, `FileGeneration`, `DTOGenerator(.Server/.Audit)`, `BLLGenerator`, `BLLCoreGenerator`, `ServiceModelGenerator`, `WebApiGenerator`, `ProjectionGenerator` |
| `_Authorization` | Authorization subsystem (Domain, BLL, Generated.DAL/DTO, WebApi, TestGenerate) |
| `_Configuration` | Configuration subsystem (same structure as `_Authorization`) |
| `_SampleSystem` | **Reference application** plus all integration/generation tests. The best example of how the framework is used |
| `__SolutionItems` | Shared solution files |

The `_` prefix marks application-level systems built on the framework (Authorization, Configuration, SampleSystem). They are also the consumers of code generation.

## Code generation (important)

Many files are **generated** and checked into the repository (some are in `.gitignore`, e.g. `SampleSystem.Generated.cs`, `*.Event.Generated.cs`, `*.Projections.Generated.cs`, the DTO csproj). **Do not edit `*.Generated.cs`, `Generated.*` projects or `*.hbm.xml` by hand** — change the generator or the source domain model and regenerate.

Generators are run from these test/utility projects:
- `_Authorization/Framework.Authorization.TestGenerate` — `ServerGenerators`
- `_Configuration/Framework.Configuration.TestGenerate` — `ServerGenerators`
- `_SampleSystem/_Tests/SampleSystem.CodeGenerate` — `ServerGenerators` (+ `Configurations/`)
- DB/migrations: `_SampleSystem/_Tests/SampleSystem.DbGenerate(.NHibernate|.EntityFramework)`

The CI check `SampleSystem.CheckGenTests/GenerationTests` fails if regeneration produces new or modified files, i.e. generated code in the repo must be up to date. After changing generators or the domain model, run generation and include the result in the changes.

`CsprojValidationTests` enforces csproj conventions, notably `PackageId` == `Luxoft.<ProjectName>`.

## Build and test

Run from `src/` (or pass the path to the `.slnx`). Platform is Windows; to filter `dotnet build ... | grep` output use the Bash tool, not PowerShell redirection.

```bash
dotnet build src/BssFramework.slnx -c Release
```

Tests (as in CI, `.github/workflows/build+unit.yml`):

```bash
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.CheckGenTests
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.UnitTests
dotnet test --project src/Framework.ExpressionParsers.Tests
dotnet test --project src/Validation/Framework.Validation.Tests
dotnet test --project src/Application/Framework.Application.Tests
dotnet test --project src/Notification/Framework.Notification.Tests
dotnet test --project src/Database/Framework.Database.NHibernate.DBGenerator.Tests
```

Integration tests (require SQL Server; `integration-tests.yml`):
`src/_SampleSystem/_Tests/SampleSystem.IntegrationTests.EntityFramework` and `...IntegrationTests.NHibernate`. Shared code is in `SampleSystem.IntegrationTests`. Docs: `docs/integrationtests.md`.

Avoid full solution builds unless necessary — the solution is large (~140 projects); build the specific project you changed. The exception is **nullable warnings**: only a full solution build with `--no-incremental` is authoritative (per-project counts are unreliable).

## Code conventions

- Nullable is enabled everywhere. The solution currently has **0 nullable warnings**; do not introduce new ones.
  - In domain assemblies / Models / attributes, do not use `required`. Mandatory fields (Master references, `[Required]`, `.Not.Nullable()` in FluentNHibernate mapping) use `= null!`; optional ones use `Type?`.
  - Reflection results (`GetMethod/GetProperty/ReflectedType`) — use `!`.
  - Generic keys/values that are never null → `where T : notnull` / `class`.
  - `x == null` with an overloaded `operator ==` → use `is null`.
  - `GetById(id, throwOnNotFound: true)!`.
  - **Do not change runtime behavior just to silence a warning** (use `value!`, not `?? fallback`).
- Style: follow the surrounding code (naming, comment density, idioms). Files carry a BOM — do not strip it.
- New projects: `PackageId` = `Luxoft.<ProjectName>` (or omit it); package versions go through `Directory.Packages.props`.
- Extension points go through DI (`ServiceCollectionExtensions`, `Framework.DependencyInjection`) and `*.Abstractions` contracts; implementations live in `*.Runtime`. `*.Legacy*` is the deprecated API — do not use it in new code.

## Navigating the code

- `graphify-out/` holds a knowledge graph (`GRAPH_REPORT.md`, `graph.json`) for a quick overview of communities and relationships. It may be stale — compare `git rev-parse HEAD` with the commit in the report. Update with `graphify update .`.
- Use Grep/Glob for search; ignore `bin/`, `obj/`, `TestResults/`, `.vs/`.
- To learn how a subsystem is used, look at `src/_SampleSystem`.
- Typical SampleSystem chain: `SampleSystem.Domain` (entities) → `*.Generated.DAL.{NHibernate|EntityFramework}` (mapping) → `SampleSystem.BLL(.Core)` (business logic, generated BLL) → `SampleSystem.Generated.DTO` → `SampleSystem.WebApiCore(.NHibernate|.EntityFramework)` (controllers) → `SampleSystem.ServiceEnvironment(.*)` (DI composition).

## Working in this repo

- Do not hand-edit generated files, `TestResults/`, `tmp_prod_projects.json`, or `*.DotSettings.user`.
