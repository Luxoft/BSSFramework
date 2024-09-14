using Framework.Authorization.Domain;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Services;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationRunAsManager(
    AuthorizationPermissionSystem authorizationPermissionSystem,
    IUserAuthenticationService userAuthenticationService,
    [DisabledSecurity] IRepository<Principal> principalRepository,
    ICurrentPrincipalSource currentPrincipalSource)
    : RunAsManager(userAuthenticationService)
{
    private Principal CurrentPrincipal => currentPrincipalSource.CurrentPrincipal;

    public override string? RunAsName => this.CurrentPrincipal.RunAs?.Name;

    protected override async Task PersistRunAs(string? principalName, CancellationToken cancellationToken)
    {
        this.CurrentPrincipal.RunAs =
            principalName == null
                ? null
                : await principalRepository.GetQueryable().SingleOrDefaultAsync(p => p.Name == principalName, cancellationToken)
                  ?? throw new Exception($"Principal with name '{principalName}' not found");

        await principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
    }

    protected override void CheckAccess() => authorizationPermissionSystem.GetPermissionSource(SecurityRole.Administrator, false).HasAccess();
}
