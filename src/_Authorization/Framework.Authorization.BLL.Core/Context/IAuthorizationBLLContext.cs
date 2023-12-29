using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

public partial interface IAuthorizationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>,

    IConfigurationBLLContextContainer<IConfigurationBLLContext>
{
    string CurrentPrincipalName => this.AuthorizationSystem.CurrentPrincipalName;

    IActualPrincipalSource ActualPrincipalSource { get; }

    IRunAsManager RunAsManager { get; }

    IAuthorizationSystem<Guid> AuthorizationSystem { get; }

    IAvailablePermissionSource AvailablePermissionSource { get; }

    IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; }

    ISecurityOperationParser<Guid> SecurityOperationParser { get; }

    TimeProvider TimeProvider { get; }

    IAuthorizationExternalSource ExternalSource { get; }

    INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    Principal CurrentPrincipal { get; }

    Settings Settings { get; }


    EntityType GetEntityType(Type type);

    EntityType GetEntityType(string domainTypeName);

    EntityType GetEntityType(Guid domainTypeId);


    /// <summary>
    /// Получение форматированного вида пермиссии
    /// </summary>
    /// <param name="permission">Пермиссия</param>
    /// <param name="separator">Разделитель</param>
    /// <returns></returns>
    string GetFormattedPermission(Permission permission, string separator = " | ");
}
