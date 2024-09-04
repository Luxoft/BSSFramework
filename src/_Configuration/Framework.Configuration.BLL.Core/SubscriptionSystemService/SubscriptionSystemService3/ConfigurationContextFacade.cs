using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Notification;
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

    public IServiceProvider ServiceProvider => this.context.ServiceProvider;

    /// <summary>
    ///     Преобразует список экземпляров <see cref="Principal" /> в список экземпляров <see cref="IEmployee" />.
    /// </summary>
    /// <param name="principals">Cписок экземпляров <see cref="Principal" />.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{IEmployee}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент principals равен null.</exception>
    public virtual IEnumerable<IEmployee> ConvertPrincipals(IEnumerable<Principal> principals)
    {
        if (principals == null)
        {
            throw new ArgumentNullException(nameof(principals));
        }

        var principalNames = principals.ToList(principal => principal.Name);

        var employees = this.context
                            .EmployeeSource
                            .GetQueryable()
                            .Where(employee => principalNames.Contains(employee.Login))
                            .ToList();

        return employees;
    }

    /// <summary>
    ///     Возращает тип контекста безопасности доменного типа.
    /// </summary>
    /// <param name="securityContextType">Описатель доменного типа.</param>
    /// <returns>Экземпляр <see cref="Type" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент securityContextType равен null.</exception>
    public virtual Type GetSecurityType(SecurityContextType securityContextType)
    {
        if (securityContextType == null)
        {
            throw new ArgumentNullException(nameof(securityContextType));
        }

        var result = this.context.ServiceProvider.GetRequiredService<ISecurityContextSource>().GetSecurityContextInfo(securityContextType.Id).Type;
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
    /// <param name="securityRoles"></param>
    /// <param name="notificationFilterGroups">Список фильтров получателей.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{Principal}" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Аргумент
    ///     roleIdents
    ///     или
    ///     notificationFilterGroups равен null.
    /// </exception>
    public virtual IEnumerable<Principal> GetNotificationPrincipals(
        SecurityRole[] securityRoles,
        IEnumerable<NotificationFilterGroup> notificationFilterGroups)
    {
        if (securityRoles == null)
        {
            throw new ArgumentNullException(nameof(securityRoles));
        }

        if (notificationFilterGroups == null)
        {
            throw new ArgumentNullException(nameof(notificationFilterGroups));
        }

        var result = this.context.Authorization.NotificationPrincipalExtractor.GetNotificationPrincipalsByRoles(securityRoles, notificationFilterGroups);

        return result;
    }

    /// <summary>
    ///     Возвращает список получателей уведомлений.
    /// </summary>
    /// <param name="securityRoles"></param>
    /// <returns>Экземпляр <see cref="IEnumerable{Principal}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент roleIdents равен null.</exception>
    public virtual IEnumerable<Principal> GetNotificationPrincipals(SecurityRole[] securityRoles)
    {
        if (securityRoles == null)
        {
            throw new ArgumentNullException(nameof(securityRoles));
        }

        var result = this.context.Authorization.NotificationPrincipalExtractor.GetNotificationPrincipalsByRoles(securityRoles, Array.Empty<NotificationFilterGroup>());

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
