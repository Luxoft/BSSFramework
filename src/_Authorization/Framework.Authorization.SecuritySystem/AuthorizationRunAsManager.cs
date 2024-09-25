using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Services;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationRunAsManager(
    IUserAuthenticationService userAuthenticationService,
    ISecuritySystemFactory securitySystemFactory,
    ICurrentPrincipalSource currentPrincipalSource,
    [DisabledSecurity] IRepository<Principal> principalRepository,
    IEnumerable<IRunAsValidator> validators)
    : RunAsManager(userAuthenticationService, securitySystemFactory)
{
    private Principal CurrentPrincipal => currentPrincipalSource.CurrentPrincipal;

    public override string? RunAsName => this.CurrentPrincipal.RunAs?.Name;

    protected override async Task PersistRunAs(string? principalName, CancellationToken cancellationToken)
    {
        if (principalName != null)
        {
            validators.Foreach(validator => validator.Validate(principalName));
        }

        this.CurrentPrincipal.RunAs =
            principalName == null
                ? null
                : await principalRepository.GetQueryable().SingleOrDefaultAsync(p => p.Name == principalName, cancellationToken)
                  ?? throw new Exception($"Principal with name '{principalName}' not found");

        await principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
    }
}
