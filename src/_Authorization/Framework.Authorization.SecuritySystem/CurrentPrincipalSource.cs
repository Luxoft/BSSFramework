using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class CurrentPrincipalSource(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    IUserAuthenticationService userAuthenticationService) : ICurrentPrincipalSource
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
