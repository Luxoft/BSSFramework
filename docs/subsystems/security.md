# Security (Role & Context Base Security)

The access model in BSS Framework is built on roles and contexts:

- A **role** (`SecurityRole`) defines *what* a user can do.
- A **context** (`ISecurityContext`, e.g. `BusinessUnit`, `Location`) defines *over which objects*.
- A **permission** links a user (principal) with a role and context restrictions. For example, "the SE Manager role for business unit X and its descendants".
- An **operation** (`SecurityOperation`) is an atomic action (`EmployeeView`, `EmployeeEdit`). A role includes a set of operations.
- A **rule** (`SecurityRule`) is a condition for accessing a domain type: a set of roles or operations the user must have.

The implementation is in the `Anch.SecuritySystem*` packages (an external dependency); BSS Framework wires it in, embeds it into the BLL / repositories and generates secure queries. This document describes **configuration** in an application. For algorithm details see the `Anch.SecuritySystem` documentation.

All examples are taken from `src/_SampleSystem/SampleSystem.Security` and `SampleSystem.Security.Abstract`.

## Where things are defined

| What | Where | Example |
|---|---|---|
| Identifiers of roles, operations, rules (constants) | `SampleSystem.Security.Abstract` | `SampleSystemSecurityRole.SeManager`, `SampleSystemSecurityOperation.EmployeeView` |
| Registration of contexts, roles, rules, virtual permissions | `SampleSystem.Security/SampleSystemSecuritySystemExtensions.cs` | `AddSecurityContexts`, `AddSecurityRoles`, … |
| Wiring into the application | `SampleSystem.ServiceEnvironment/DependencyInjection/SampleSystemGeneralDependencyInjectionExtensions.cs` | `AddSecuritySystem(...)` |

Identifiers live in a separate `*.Abstract` project so that code generators and client projects can use them without depending on the runtime.

## Contexts

A context is a domain type marked with `ISecurityContext`, with a permanent `Guid`:

```csharp
settings.AddSecurityContext<BusinessUnit>(
    new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"),
    b => b.SetHierarchicalInfo(
        v => v.Parent,
        new AncestorLinkInfo<BusinessUnit, BusinessUnitAncestorLink>(link => link.Ancestor, link => link.Child),
        new AncestorLinkInfo<BusinessUnit, BusinessUnitToAncestorChildView>(link => link.Source, link => link.ChildOrAncestor),
        v => v.DeepLevel))
```

- The `Guid` is stored in the DB together with permissions, **it must not be changed** after release.
- `SetHierarchicalInfo` describes the context hierarchy: the parent, the ancestor tables / views and the nesting level. Thanks to this, a permission on a parent extends to its descendants.
- For a non-hierarchical context `SetDisplayFunc` is enough (as for `Employee`, displayed by `Login`).
- Hierarchical types must also be listed in `ServerGenerationEnvironment.IsHierarchical` (see [codegeneration.md](../codegeneration.md)).

Ancestor tables (`*AncestorLink`) are maintained by named locks (`AddNamedLocks(...AddManual(typeof(BusinessUnitAncestorLink)))`).

## Roles

```csharp
.AddSecurityRole(
    SampleSystemSecurityRole.SeManager,
    new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"))
    {
        Children = [SampleSystemSecurityRole.TestVirtualRole],
        Operations = [SampleSystemSecurityOperation.BusinessUnitEdit]
    })
```

`SecurityRoleInfo` parameters (from the examples in `SampleSystem.Security`):

| Parameter | Meaning |
|---|---|
| `Guid` (constructor) | Permanent role identifier |
| `Operations` | Operations the role grants |
| `Children` | Nested roles (a role includes the rights of its children) |
| `IsVirtual` | Virtual role (not granted directly, computed) |
| `Restriction` | Which contexts the role requires or allows: `SecurityPathRestriction.Create<BusinessUnit>(required: true, filter: …).Add<Location>(true)` |

In SampleSystem the `SecurityRole.Administrator` role gets all operations of the application. `SecurityRole.SystemIntegration` is intended for integration calls.

## Rules

A rule describes the condition for accessing a domain type:

```csharp
settings.AddSecurityRule(
    SampleSystemSecurityGroup.TestGroup,
    SecurityRule.View.ToDomain<Employee>());
```

- `SecurityRule.View` / `Edit` / `Disabled` are built-in rules.
- `.ToDomain<T>()` and `.And((T v) => v.Prop)` bind the rule to a type and to an extra condition on the object.
- A repository with access checks is created like this: `businessUnitRepository.Create(SecurityRule.View)`. A repository without checks is obtained via `[DisabledSecurity] IRepository<T>` (use it for system tasks and jobs).

## Operations with hierarchy expansion

```csharp
.AddSecurityOperation(
    SampleSystemSecurityOperation.BusinessUnitView,
    new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All })
```

`CustomExpandType` defines how a permission on a hierarchy node extends to descendants and ancestors.

## Virtual permissions

If a permission can be derived from application data rather than from the permissions table, use a virtual permission:

```csharp
settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
    link => link.Employee,
    vpb => vpb.AddRestriction(link => link.BusinessUnit)
              .AddSecurityRole(
                  SampleSystemSecurityRole.SeManager,
                  v => v.AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager)));
```

Here any employee linked to a business unit with the `Manager` role is considered to hold `SeManager` for that unit.

## Wiring into the application

In `AddGeneralDependencyInjection` everything is assembled in a single call:

```csharp
rootSettings.AddSecuritySystem(securitySettings => securitySettings
    .AddSecurityContexts()
    .AddDomainSecurityServices()
    .AddSecurityRoles()
    .AddSecurityRules()
    .AddCustomSecurityOperations()
    .SetClientDomainModeSecurityRuleSource<SampleSystemClientDomainModeSecurityRuleSource>()
    .AddClientSecurityRuleInfoSource(typeof(SampleSystemSecurityGroup))
    .AddUserSource<Employee>(usb => usb.SetFilter(employee => employee.Active))
    .AddVirtualPermissions()
    .SetSecurityAdministratorRule(SampleSystemSecurityRole.PermissionAdministrator)
    .AddAuthorizationSystem()
    .AddConfigurationSecurity()
    .AddPrincipalManagementListener<SamplePrincipalManagementListener>());
```

Key points:

- `AddUserSource<Employee>` links security users to a domain entity. The filter excludes inactive ones.
- `SetSecurityAdministratorRule` sets the role that can manage permissions.
- `AddAuthorizationSystem` and `AddConfigurationSecurity` plug in the built-in `_Authorization` and `_Configuration` subsystems.
- `AddPrincipalManagementListener` lets you react to principal changes (see `SamplePrincipalManagementListener`).
- In Swagger, client rules are published via `AddClientSecurityRule("SampleSystemSecurityRule")`.

## Property-level security

Access can be restricted not only to a type but to an individual property: with the `[ViewDomainObject]` and `[EditDomainObject]` attributes. They can be placed directly on the model or added from outside via the generation environment's extended attributes (`GetExtendedAttributeSources`), without touching the class:

```csharp
.Add<Employee>(b => b.AddProperty(
    e => e.Login,
    pb => pb.AddAttribute(new ViewDomainObjectAttribute(SecurityRule.View))
            .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeeEdit))))
```

The attributes are taken into account when generating DTOs and services.

## Security in queries and DTOs

- Domain types that depend on a context declare this at the model level so that filtering reaches the query and the DB (for example, `[DependencySecurity]` for projections).
- Tests that check scenarios (`SecurityRuleTests`, `SecurityPathTests`, `RestrictionRoleTests`, `DependencySecurityTests`, `VirtualPermissionTests`, etc.) are in `src/_SampleSystem/_Tests/SampleSystem.IntegrationTests` and are the best examples.

## Tests and permissions

In integration tests roles are granted via `AuthManager`: `this.AuthManager.For("user").SetRoleAsync(role, ct)` / `SetAdminRoleAsync(ct)` (see [integrationtests.md](../integrationtests.md)).
