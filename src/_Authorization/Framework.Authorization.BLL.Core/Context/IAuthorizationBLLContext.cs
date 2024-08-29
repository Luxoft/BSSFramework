using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem;
using Framework.SecuritySystem.UserSource;

namespace Framework.Authorization.BLL;

public partial interface IAuthorizationBLLContext :

    ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>,

    ITrackingServiceContainer<PersistentDomainObjectBase>,

    ITypeResolverContainer<string>
{
    IPrincipalGeneralValidator PrincipalValidator { get; }

    ICurrentPrincipalSource CurrentPrincipalSource { get; }

    ICurrentUser CurrentUser { get; }

    IRunAsManager RunAsManager { get; }

    ISecuritySystem SecuritySystem { get; }

    IAvailablePermissionSource AvailablePermissionSource { get; }

    IAvailableSecurityRoleSource AvailableSecurityRoleSource { get; }

    IAvailableSecurityOperationSource AvailableSecurityOperationSource { get; }

    TimeProvider TimeProvider { get; }

    IAuthorizationExternalSource ExternalSource { get; }

    INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    SecurityContextType GetSecurityContextType(Type type);

    SecurityContextType GetSecurityContextType(string domainTypeName);

    SecurityContextType GetSecurityContextType(Guid domainTypeId);
}
