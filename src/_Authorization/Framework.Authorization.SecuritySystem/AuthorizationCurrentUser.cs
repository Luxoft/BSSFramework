using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem.UserSource;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationCurrentUser(ICurrentPrincipalSource currentPrincipalSource) : ICurrentUser
{
    public string Name => this.ActualPrincipal.Name;

    private Principal ActualPrincipal => currentPrincipalSource.CurrentPrincipal.Pipe(v => v.RunAs ?? v);
}
