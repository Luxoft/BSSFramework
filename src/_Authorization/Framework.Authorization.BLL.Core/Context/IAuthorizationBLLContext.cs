using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystemImpl;
using Framework.Authorization.SecuritySystemImpl.Validation;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using SecuritySystem;
using SecuritySystem.AvailableSecurity;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.Services;

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

    ISecurityContextStorage SecurityContextStorage { get; }

    ISecurityContextInfoSource SecurityContextInfoSource { get; }

    INotificationPrincipalExtractor NotificationPrincipalExtractor { get; }

    SecurityContextType GetSecurityContextType(Type type);
}
