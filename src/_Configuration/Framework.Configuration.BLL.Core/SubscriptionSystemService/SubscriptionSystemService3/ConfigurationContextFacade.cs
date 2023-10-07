using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL.SubscriptionSystemService3;

/// <summary>
///     Фасад контекста конфигурации. Упрощает доступ к методам объектов контекста конфигурации.
/// </summary>
public class ConfigurationContextFacade
{
    private readonly IConfigurationBLLContext context;

    /// <summary>
    ///     Создаёт экземпляр класса <see cref="ConfigurationContextFacade" />.
    /// </summary>
    /// <param name="context">Контекст конфигурации.</param>
    /// <exception cref="ArgumentNullException">Аргумент context равен null.</exception>
    public ConfigurationContextFacade(IConfigurationBLLContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        this.context = context;
    }

    /// <summary>
    ///     Преобразует список экземпляров <see cref="IPrincipal{Guid}" /> в список экземпляров <see cref="IEmployee" />.
    /// </summary>
    /// <param name="principals">Cписок экземпляров <see cref="IPrincipal{Guid}" />.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{IEmployee}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент principals равен null.</exception>
    public virtual IEnumerable<IEmployee> ConvertPrincipals(IEnumerable<IPrincipal<Guid>> principals)
    {
        if (principals == null)
        {
            throw new ArgumentNullException(nameof(principals));
        }

        var principalNames = principals.ToList(principal => principal.Name);

        var employees = this.context.GetEmployeeSource()
                            .GetUnsecureQueryable()
                            .Where(employee => principalNames.Contains(employee.Login))
                            .ToList();

        return employees;
    }

    /// <summary>Возращает тип контекста безопасности доменного типа.</summary>
    /// <param name="authDomainTypeId">Идентификатор доменного типа контекста безопасности.</param>
    /// <returns>
    ///     Экземпляр <see cref="Type" />, тип контекста безопасности доменного типа.
    ///     Или null, если тип по предоставленому <paramref name="authDomainTypeId" />
    ///     тип контекста безопасности доменного типа не найден.
    /// </returns>
    public virtual Type GetSecurityType(Guid authDomainTypeId)
    {
        var entityType = this.context.Authorization.Logics.EntityType.GetById(authDomainTypeId);

        if (entityType == null)
        {
            return null;
        }

        var result = this.GetSecurityType(entityType);

        return result;
    }

    /// <summary>
    ///     Возращает тип контекста безопасности доменного типа.
    /// </summary>
    /// <param name="entityType">Описатель доменного типа.</param>
    /// <returns>Экземпляр <see cref="Type" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент entityType равен null.</exception>
    public virtual Type GetSecurityType(EntityType entityType)
    {
        if (entityType == null)
        {
            throw new ArgumentNullException(nameof(entityType));
        }

        var result = this.context.ServiceProvider.GetRequiredService<ISecurityContextInfoService>().GetSecurityContextInfo(entityType.Name).Type;
        return result;
    }

    /// <summary>
    ///     Возвращает описатель сущности в котексте которой выдаются права пользователю.
    /// </summary>
    /// <param name="domainTypeName">Имя доменного типа.</param>
    /// <returns>Экземпляр <see cref="EntityType" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент domainTypeName равен null.</exception>
    public virtual EntityType GetEntityType(string domainTypeName)
    {
        if (domainTypeName == null)
        {
            throw new ArgumentNullException(nameof(domainTypeName));
        }

        var result = this.context.Authorization.GetEntityType(domainTypeName);
        return result;
    }

    /// <summary>
    ///     Возвращает описатель типа доменного объекта.
    /// </summary>
    /// <param name="domainObjectType">Тип для которого будет произведен поиск описателя доменного объекта.</param>
    /// <returns>Экземпляр <see cref="DomainType" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент domainObjectType равен null.</exception>
    public virtual DomainType GetDomainType(Type domainObjectType)
    {
        if (domainObjectType == null)
        {
            throw new ArgumentNullException(nameof(domainObjectType));
        }

        return this.context.GetDomainType(domainObjectType, true);
    }

    /// <summary>
    ///     Возвращает реальный тип, связанный с описателем типа доменного объекта.
    /// </summary>
    /// <param name="description">Описатель типа доменного типа.</param>
    /// <returns>Экземпляр <see cref="Type" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент description равен null.</exception>
    /// <exception cref="SubscriptionServicesException">
    ///     Реальный тип, соответствующий описанию доменного типа, не найден.
    /// </exception>
    public virtual Type GetDomainObjectType(TypeInfoDescription description)
    {
        if (description == null)
        {
            throw new ArgumentNullException(nameof(description));
        }

        var domainType = this.context.GetDomainType(description);
        var result = this.GetDomainObjectType(domainType);

        return result;
    }

    /// <summary>
    ///     Возвращает реальный тип, связанный с описателем доменного объекта.
    /// </summary>
    /// <param name="domainType">Описатель доменного типа.</param>
    /// <returns>Экземпляр <see cref="Type" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент domainType равен null.</exception>
    /// <exception cref="SubscriptionServicesException">
    ///     Реальный тип, соответствующий описанию доменного типа, не найден.
    /// </exception>
    public virtual Type GetDomainObjectType(DomainType domainType)
    {
        if (domainType == null)
        {
            throw new ArgumentNullException(nameof(domainType));
        }

        var result = this.context.ComplexDomainTypeResolver.Resolve(domainType);

        if (result == null)
        {
            throw new SubscriptionServicesException($"Type for DomainType '{domainType}' not found.");
        }

        return result;
    }

    /// <summary>
    ///     Возвращает список получателей уведомлений.
    /// </summary>
    /// <param name="roleIdents">Идентификаторы ролей получателей.</param>
    /// <param name="notificationFilterGroups">Список фильтров получателей.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{Principal}" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     roleIdents
    ///     или
    ///     notificationFilterGroups равен null.
    /// </exception>
    public virtual IEnumerable<Principal> GetNotificationPrincipals(
            Guid[] roleIdents,
            IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (roleIdents == null)
        {
            throw new ArgumentNullException(nameof(roleIdents));
        }

        if (notificationFilterGroups == null)
        {
            throw new ArgumentNullException(nameof(notificationFilterGroups));
        }

        var result = this.context.Authorization.NotificationPrincipalExtractor.GetNotificationPrincipalsByRoles(roleIdents, notificationFilterGroups);

        return result;
    }

    /// <summary>
    ///     Возвращает список получателей уведомлений.
    /// </summary>
    /// <param name="roleIdents">Идентификаторы ролей получателей.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{Principal}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент roleIdents равен null.</exception>
    public virtual IEnumerable<Principal> GetNotificationPrincipals(Guid[] roleIdents)
    {
        if (roleIdents == null)
        {
            throw new ArgumentNullException(nameof(roleIdents));
        }

        var result = this.context.Authorization.NotificationPrincipalExtractor.GetNotificationPrincipalsByRoles(roleIdents, Array.Empty<NotificationFilterGroup>());

        return result;
    }


    /// <summary>
    /// Возвращает коды активных code first подписок.
    /// </summary>
    /// <returns>Список кодов активных code first подписок.</returns>
    public virtual IEnumerable<string> GetActiveCodeFirstSubscriptionCodes()
    {
        var result = this.context.Logics.CodeFirstSubscription.GetActiveCodeFirstSubscriptionCodes();
        return result;
    }
}
