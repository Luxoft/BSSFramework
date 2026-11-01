# Data audit

The framework supports two independent audit mechanisms. They solve different problems and can be used together.

| | Inline audit | Envers audit (version history) |
|---|---|---|
| What it stores | Who created / modified the object and when: fields directly in the entity row | The full change history: a copy of the row for each revision |
| Where it is stored | Columns of the same table (`CreateDate`, `CreatedBy`, `ModifyDate`, `ModifiedBy`) | Separate audit tables and a revisions table, a separate `DbContext` / schema |
| Read | Like regular fields | Through audit controllers (`mainAuditApi/…`) and `Audit` DTOs |
| ORM | NHibernate and EF | NHibernate (`NHibernate.Envers`) and EF (own implementation `Framework.Database.EntityFramework.EnversAudit`) |

## Inline audit

### Model

The entity inherits `AuditPersistentDomainObjectBase`. It adds the fields `CreateDate`, `CreatedBy`, `ModifyDate`, `ModifiedBy`, `Active` and `Version` (`IVersionObject<long>`). The setters are `protected internal`: application code does not fill them, the framework does it on save.

### Setup

The setup is in `SampleSystem.ServiceEnvironment/DependencyInjection/SampleSystemInlineAuditSetup.cs`:

```csharp
public static IInlineAuditSetup AddSampleSystemInlineAudit(this IInlineAuditSetup rootSetup) =>
    rootSetup
        .AddAuthorizationInlineAudit()
        .AddConfigurationInlineAudit()
        .AddDefault<SampleSystem.Domain.AuditPersistentDomainObjectBase>(
            v => v.CreateDate, v => v.CreatedBy, v => v.ModifyDate, v => v.ModifiedBy)
        .For<SampleSystem.Domain.ExtendedInlineAuditObj>(innerSetup =>
        {
            innerSetup.Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.CreatedByEmployee, InlineAuditAction.Create)
                      .Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.ModifiedByEmployee, InlineAuditAction.Modify);
        });
```

- `AddDefault<TBase>(createDate, createdBy, modifyDate, modifiedBy)` sets the standard fields for all descendants of the base class.
- `For<T>(...)` adds custom fields with value resolvers to a specific type.
- `InlineAuditAction` (`Create`, `Modify`) determines when the field is filled.
- The field value is computed by `IAuditValueResolver<T>`. Ready-made implementations: `NowAuditValueResolver` (current time), `CurrentUserAuditValueResolver` (current user); you can write your own.

### Wiring to the ORM

In EF (`SampleSystemEntityFrameworkExtension`):

```csharp
options.AddInlineAudit(s => s.SetSetupType<SampleSystemInlineAuditSetup>())
```

The type `SampleSystemInlineAuditSetup` implements `IInlineAuditExtensionInnerSetup` and calls `AddSampleSystemInlineAudit()`. NHibernate (`SampleSystemNHibernateExtension`) does the same: `.AddInlineAudit(rootSetup => rootSetup.AddSampleSystemInlineAudit())`.

## Envers audit

Stores versions of every change: a revision record (who, when) and a copy of the changed data in audit tables.

### What is audited

- In EF it is all entities except projections and types with `[NotAuditedClass]`:

```csharp
setup.SetFilter(et => !et.ClrType.IsProjection() && !et.ClrType.HasAttribute<NotAuditedClassAttribute>());
```

- The audit type in SampleSystem is described in `SampleSystem.AuditDomain` (`BusinessUnitAudit`, `SampleSystemAuditRevisionEntity`), and the mapping is generated (`Mapping/Envers`).
- To exclude an entity from audit: `[NotAuditedClass]` from `Framework.Database.Mapping`.

### Wiring (EF)

```csharp
.AddDbContext<SampleSystemDbContext>((sp, options) => options
    .UseSqlServer(...)
    .AddEnversAudit(s => s.SetSetupType<SampleSystemEnversAuditSetup>()))

.AddDbContext<SampleSystemEnversAuditDbContext>(...)       // a separate context for reading audit

.AddEntityFramework<SampleSystemDbContext>(s => s.AddEnversAudit()
                                                 .AddSecondaryContext<SampleSystemEnversAuditDbContext>());
```

`SampleSystemDbContext` implements `IEnversAuditDbContext`, and `SampleSystemEnversAuditDbContext` contains the audit table configurations (`BusinessUnitAuditMap`, `SampleSystemAuditRevisionEntityMap`).

### Reading history

Reading uses audit controllers and audit DTOs, which are generated separately (`AuditDTOGeneratorConfiguration`, `AuditServiceGeneratorConfiguration`, route `mainAuditApi/[controller]/[action]`, see [codegeneration.md](../codegeneration.md)). In code, reading is done by `IEfAuditReader` (EF).

### DB schema

Audit tables are created during schema generation (for NHibernate: `AuditDBGenerator/AuditDatabaseScriptGenerator`). For migrations and schema comparison see the "DB schema" section in [codegeneration.md](../codegeneration.md).

## Tracking

`Framework.Tracking` (contracts in `Framework.Tracking.Abstractions`: `ITrackingService<T>`, `TrackingResult<T>`, `TrackingProperty`, `GetChangesMode`) determines which persistent properties of an object changed in the current session: property name, old and new value. Use it in the BLL and validators when you need to know exactly what changed, without going to audit. Tests are in `Framework.Tracking.Tests`.

## Tests

Integration tests: `AuditTests`, `EnversTests`, `EnversBug1676`, `InlineAuditTests` (`SampleSystem.IntegrationTests`).
