﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class CurrentPrincipalSource : ICurrentPrincipalSource
{
    private readonly IRepository<Principal> principalRepository;

    private readonly IUserAuthenticationService userAuthenticationService;

    private readonly Lazy<Principal> currentPrincipalLazy;

    public CurrentPrincipalSource([FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<Principal> principalRepository,
                                  IUserAuthenticationService userAuthenticationService)
    {
        this.principalRepository = principalRepository;
        this.userAuthenticationService = userAuthenticationService;

        var userName = this.userAuthenticationService.GetUserName();

        this.currentPrincipalLazy = LazyHelper.Create(
            () => this.principalRepository
                      .GetQueryable().SingleOrDefault(principal => principal.Active && principal.Name == userName) ?? new Principal { Name = userName });
    }

    public Principal CurrentPrincipal => this.currentPrincipalLazy.Value;
}
