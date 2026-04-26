using Anch.Core.Auth;

using Framework.Authorization.Domain;
using Framework.BLL;
using Framework.Validation;

using Anch.SecuritySystem;
using Anch.SecuritySystem.ExternalSystem.SecurityContextStorage;
using Anch.SecuritySystem.GeneralPermission.Validation.Principal;
using Anch.SecuritySystem.Services;
using Anch.SecuritySystem.UserSource;

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
