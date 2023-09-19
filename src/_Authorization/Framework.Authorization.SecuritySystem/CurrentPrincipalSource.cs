using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;

namespace Framework.Authorization.SecuritySystem;

public class CurrentPrincipalSource : ICurrentPrincipalSource
{
    private readonly IRepositoryFactory<Principal> principalRepository;

    private readonly IUserAuthenticationService userAuthenticationService;

    private readonly Lazy<Principal> currentPrincipalLazy;

    public CurrentPrincipalSource(IRepositoryFactory<Principal> principalRepository,
                                  IUserAuthenticationService userAuthenticationService)
    {
        this.principalRepository = principalRepository;
        this.userAuthenticationService = userAuthenticationService;

        var userName = this.userAuthenticationService.GetUserName();

        this.currentPrincipalLazy = LazyHelper.Create(
            () => this.principalRepository.Create()
                      .GetQueryable().SingleOrDefault(principal => principal.Active && principal.Name == userName) ?? new Principal { Name = userName });
    }

    public Principal CurrentPrincipal => this.currentPrincipalLazy.Value;
}
