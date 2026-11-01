[![license](https://img.shields.io/github/license/luxoft/BSSFramework)](https://github.com/Luxoft/BSSFramework/blob/main/LICENSE)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Luxoft_BSSFramework&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Luxoft_BSSFramework)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Luxoft_BSSFramework&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Luxoft_BSSFramework)
![Nuget](https://img.shields.io/nuget/v/Luxoft.Framework.Core)
[![Build+Unit](https://github.com/Luxoft/BSSFramework/actions/workflows/build+unit.yml/badge.svg)](https://github.com/Luxoft/BSSFramework/actions/workflows/build+unit.yml)
[![Integration tests](https://github.com/Luxoft/BSSFramework/actions/workflows/integration-tests.yml/badge.svg)](https://github.com/Luxoft/BSSFramework/actions/workflows/integration-tests.yml)


# BSS Framework
Platform for rapid software development

Features:
- [**Role&Context Base Security**](docs/subsystems/security.md)
- **Column Base Restrictions**
- [**Notification**](docs/subsystems/notification.md) and subscriptions
- **Reporting**
- **Integration**
- **Hangfire** https://www.hangfire.io/ ([jobs](docs/subsystems/jobs.md))
- **CAP** https://cap.dotnetcore.xyz/
- [**Data Audit**](docs/subsystems/audit.md) (Inline + [Envers](https://github.com/nhibernate/nhibernate-envers))
- **Logging**
- [**Validation**](docs/subsystems/validation.md)
- [**Code Generation**](docs/codegeneration.md)
    - DataBase Generation
    - DTO/Mapping Generation
    - ORM Mapping Generation
    - RestApi Generation
    - TypeScript Generator https://github.com/Luxoft/gengen
- **Unit Testing**
- [Integration Testing](docs/integrationtests.md)

Two ORMs are supported: **NHibernate** and **Entity Framework**. Stack: .NET 10, C# `latest`, nullable enabled. Packages are published to NuGet as `Luxoft.<ProjectName>`.

## Documentation

| Document | Contents |
|---|---|
| [docs/architecture.md](docs/architecture.md) | Layers, repository layout, structure of an application built on the framework, DI composition |
| [docs/codegeneration.md](docs/codegeneration.md) | What is generated, how to run generators, the `CheckGenTests` CI check, DB schema generation |
| [docs/integrationtests.md](docs/integrationtests.md) | SQL Server setup, settings, how to write and run integration tests |
| [docs/legacy.md](docs/legacy.md) | The `*.Legacy*` projects and how to recognise legacy API |
| [docs/subsystems/security.md](docs/subsystems/security.md) | Roles, contexts, rules, virtual permissions |
| [docs/subsystems/validation.md](docs/subsystems/validation.md) | Validation attributes and custom validators |
| [docs/subsystems/audit.md](docs/subsystems/audit.md) | Inline audit, Envers audit, change tracking |
| [docs/subsystems/notification.md](docs/subsystems/notification.md) | Subscriptions, templates, mail pipeline, SMTP settings |
| [docs/subsystems/jobs.md](docs/subsystems/jobs.md) | Hangfire jobs, schedules, named locks |
| [CONTRIBUTING.md](CONTRIBUTING.md) | Build, test, conventions, how to contribute |

Notes for AI agents are in [AGENTS.md](AGENTS.md).

## Quick start

The reference application is `src/_SampleSystem`. It is the best example of using the framework, and all integration tests run against it.

```bash
dotnet build src/BssFramework.slnx -c Release
dotnet test --project src/_SampleSystem/_Tests/SampleSystem.UnitTests
```

A new system on the framework is built in this order (see [architecture](docs/architecture.md) for details):

1. Describe domain entities with attributes (`*.Domain`).
2. Describe security (contexts, roles, rules), validation and subscriptions.
3. Configure the generation environment and run the generators ([codegeneration](docs/codegeneration.md)): DAL, BLL, DTO, controllers.
4. Create the DB schema with migrations.
5. Compose the host: `AddBssFramework` + the ORM-specific `IBssFrameworkExtension`.
