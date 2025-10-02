using CommonFramework;

using Framework.Authorization.Domain;

using Framework.DomainDriven.Repository;

using SecuritySystem.Attributes;
using SecuritySystem.Services;

namespace Framework.Authorization.SecuritySystem;

public class CurrentPrincipalSource(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    IRawUserAuthenticationService userAuthenticationService) : ICurrentPrincipalSource
{
    private readonly Lazy<Principal> currentPrincipalLazy = LazyHelper.Create(
        () =>
        {
            var userName = userAuthenticationService.GetUserName();

            return principalRepository
                   .GetQueryable().SingleOrDefault(principal => principal.Name == userName)
                   ?? new Principal { Name = userName };
        });

    public Principal CurrentPrincipal => this.currentPrincipalLazy.Value;
}
