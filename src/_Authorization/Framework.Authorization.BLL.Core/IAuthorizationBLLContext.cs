using CommonFramework.Auth;

using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.Validation;

using SecuritySystem;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.GeneralPermission.Validation.Principal;
using SecuritySystem.Services;
using SecuritySystem.UserSource;

namespace Framework.Authorization.BLL;

public partial interface IAuthorizationBLLContext : ISecurityBLLContext<IAuthorizationBLLContext, PersistentDomainObjectBase, Guid>
{
    IValidator Validator { get; }

    IPrincipalValidator<Principal, Permission, PermissionRestriction> PrincipalValidator { get; }

    ICurrentUserSource<Principal> CurrentPrincipalSource { get; }

    ICurrentUser CurrentUser { get; }

    IRunAsManager RunAsManager { get; }

    ISecuritySystem SecuritySystem { get; }

    ISecurityContextStorage SecurityContextStorage { get; }

    ISecurityContextInfoSource SecurityContextInfoSource { get; }

    SecurityContextType GetSecurityContextType(Type type);
}
