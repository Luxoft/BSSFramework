# Безопасность (Role & Context Base Security)

Модель доступа в BSS Framework строится на ролях и контекстах:

- **Роль** (`SecurityRole`) определяет, *что* пользователь может делать.
- **Контекст** (`ISecurityContext`, например `BusinessUnit`, `Location`) определяет, *над какими объектами*.
- **Разрешение (permission)** связывает пользователя (principal) с ролью и ограничениями по контекстам. Например, «роль SE Manager для бизнес-юнита X и его потомков».
- **Операция** (`SecurityOperation`) — атомарное действие (`EmployeeView`, `EmployeeEdit`). Роль включает набор операций.
- **Правило** (`SecurityRule`) — условие доступа к доменному типу: набор ролей или операций, которые должны быть у пользователя.

Реализация находится в пакетах `Anch.SecuritySystem*` (внешняя зависимость), а BSS Framework подключает её, встраивает в BLL / репозитории и генерирует безопасные запросы. Здесь описана **настройка** в приложении. Подробности алгоритмов смотрите в документации `Anch.SecuritySystem`.

Все примеры взяты из `src/_SampleSystem/SampleSystem.Security` и `SampleSystem.Security.Abstract`.

## Где что определяется

| Что | Где | Пример |
|---|---|---|
| Идентификаторы ролей, операций, правил (константы) | `SampleSystem.Security.Abstract` | `SampleSystemSecurityRole.SeManager`, `SampleSystemSecurityOperation.EmployeeView` |
| Регистрация контекстов, ролей, правил, виртуальных прав | `SampleSystem.Security/SampleSystemSecuritySystemExtensions.cs` | `AddSecurityContexts`, `AddSecurityRoles`, … |
| Подключение к приложению | `SampleSystem.ServiceEnvironment/DependencyInjection/SampleSystemGeneralDependencyInjectionExtensions.cs` | `AddSecuritySystem(...)` |

Идентификаторы лежат в отдельном проекте `*.Abstract`, чтобы их могли использовать генераторы кода и клиентские проекты без зависимости от рантайма.

## Контексты

Контекст — это доменный тип, помеченный `ISecurityContext`, с постоянным `Guid`:

```csharp
settings.AddSecurityContext<BusinessUnit>(
    new Guid("263D2C60-7BCE-45D6-A0AF-A0830152353E"),
    b => b.SetHierarchicalInfo(
        v => v.Parent,
        new AncestorLinkInfo<BusinessUnit, BusinessUnitAncestorLink>(link => link.Ancestor, link => link.Child),
        new AncestorLinkInfo<BusinessUnit, BusinessUnitToAncestorChildView>(link => link.Source, link => link.ChildOrAncestor),
        v => v.DeepLevel))
```

- `Guid` хранится в БД вместе с разрешениями, **его нельзя менять** после выпуска.
- `SetHierarchicalInfo` описывает иерархию контекста: родителя, таблицы / представления предков и уровень вложенности. Благодаря этому право на родителя распространяется на потомков.
- Для неиерархического контекста достаточно `SetDisplayFunc` (как для `Employee`, отображается по `Login`).
- Иерархические типы нужно также указать в `ServerGenerationEnvironment.IsHierarchical` (см. [codegeneration.md](../codegeneration.md)).

Таблицы предков (`*AncestorLink`) поддерживаются именованными блокировками (`AddNamedLocks(...AddManual(typeof(BusinessUnitAncestorLink)))`).

## Роли

```csharp
.AddSecurityRole(
    SampleSystemSecurityRole.SeManager,
    new SecurityRoleInfo(new Guid("dbf3556d-7106-4175-b5e4-a32d00bd857a"))
    {
        Children = [SampleSystemSecurityRole.TestVirtualRole],
        Operations = [SampleSystemSecurityOperation.BusinessUnitEdit]
    })
```

Параметры `SecurityRoleInfo` (по примерам в `SampleSystem.Security`):

| Параметр | Смысл |
|---|---|
| `Guid` (конструктор) | Постоянный идентификатор роли |
| `Operations` | Операции, которые даёт роль |
| `Children` | Вложенные роли (роль включает права дочерних) |
| `IsVirtual` | Виртуальная роль (не выдаётся напрямую, вычисляется) |
| `Restriction` | Какие контексты роль требует или допускает: `SecurityPathRestriction.Create<BusinessUnit>(required: true, filter: …).Add<Location>(true)` |

Роль `SecurityRole.Administrator` в SampleSystem получает все операции приложения. `SecurityRole.SystemIntegration` предназначена для интеграционных вызовов.

## Правила

Правило описывает условие доступа к доменному типу:

```csharp
settings.AddSecurityRule(
    SampleSystemSecurityGroup.TestGroup,
    SecurityRule.View.ToDomain<Employee>());
```

- `SecurityRule.View` / `Edit` / `Disabled` — встроенные правила.
- `.ToDomain<T>()` и `.And((T v) => v.Prop)` привязывают правило к типу и дополнительному условию на объект.
- Репозиторий с проверкой доступа создаётся так: `businessUnitRepository.Create(SecurityRule.View)`. Репозиторий без проверки получается через `[DisabledSecurity] IRepository<T>` (используйте для системных задач и джобов).

## Операции с расширением по иерархии

```csharp
.AddSecurityOperation(
    SampleSystemSecurityOperation.BusinessUnitView,
    new SecurityOperationInfo { CustomExpandType = HierarchicalExpandType.All })
```

`CustomExpandType` задаёт, как право на узел иерархии распространяется на потомков и предков.

## Виртуальные разрешения

Если право можно вывести из данных приложения, а не из таблицы разрешений, используйте виртуальное разрешение:

```csharp
settings.AddVirtualPermission<Employee, BusinessUnitEmployeeRole>(
    link => link.Employee,
    vpb => vpb.AddRestriction(link => link.BusinessUnit)
              .AddSecurityRole(
                  SampleSystemSecurityRole.SeManager,
                  v => v.AddFilter(link => link.Role == BusinessUnitEmployeeRoleType.Manager)));
```

Здесь любой сотрудник, связанный с бизнес-юнитом ролью `Manager`, считается обладателем `SeManager` для этого юнита.

## Подключение к приложению

В `AddGeneralDependencyInjection` всё собирается в один вызов:

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

Ключевые точки:

- `AddUserSource<Employee>` связывает пользователей безопасности с доменной сущностью. Фильтр отсекает неактивных.
- `SetSecurityAdministratorRule` задаёт роль, которая может управлять разрешениями.
- `AddAuthorizationSystem` и `AddConfigurationSecurity` подключают встроенные подсистемы `_Authorization` и `_Configuration`.
- `AddPrincipalManagementListener` позволяет реагировать на изменения принципалов (см. `SamplePrincipalManagementListener`).
- В Swagger клиентские правила публикуются через `AddClientSecurityRule("SampleSystemSecurityRule")`.

## Безопасность на уровне свойств

Доступ можно ограничить не только к типу, но и к отдельному свойству: атрибутами `[ViewDomainObject]` и `[EditDomainObject]`. Их можно поставить прямо на модель или добавить снаружи через расширенные атрибуты окружения генерации (`GetExtendedAttributeSources`), не трогая класс:

```csharp
.Add<Employee>(b => b.AddProperty(
    e => e.Login,
    pb => pb.AddAttribute(new ViewDomainObjectAttribute(SecurityRule.View))
            .AddAttribute(new EditDomainObjectAttribute(SampleSystemSecurityOperation.EmployeeEdit))))
```

Атрибуты учитываются при генерации DTO и сервисов.

## Безопасность в запросах и DTO

- Доменные типы, зависящие от контекста, объявляют это на уровне модели, чтобы фильтрация попала в запрос и в БД (например, `[DependencySecurity]` для проекций).
- Тесты, которые проверяют сценарии (`SecurityRuleTests`, `SecurityPathTests`, `RestrictionRoleTests`, `DependencySecurityTests`, `VirtualPermissionTests` и др.), лежат в `src/_SampleSystem/_Tests/SampleSystem.IntegrationTests` и служат лучшими примерами.

## Тесты и права

В интеграционных тестах роли выдаются через `AuthManager`: `this.AuthManager.For("user").SetRoleAsync(role, ct)` / `SetAdminRoleAsync(ct)` (см. [integrationtests.md](../integrationtests.md)).
