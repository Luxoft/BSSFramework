﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource : IAvailablePermissionSource
{
    private readonly IRepository<Permission> permissionRepository;

    private readonly TimeProvider timeProvider;

    private readonly IActualPrincipalSource actualPrincipalSource;

    private readonly IUserAuthenticationService userAuthenticationService;

    public AvailablePermissionSource(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<Permission> permissionRepository,
        TimeProvider timeProvider,
        IActualPrincipalSource actualPrincipalSource,
        IUserAuthenticationService userAuthenticationService)
    {
        this.permissionRepository = permissionRepository;
        this.timeProvider = timeProvider;
        this.actualPrincipalSource = actualPrincipalSource;
        this.userAuthenticationService = userAuthenticationService;
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, Guid securityRoleIdents = default, bool applyCurrentUser = true)
    {
        var filter = new AvailablePermissionFilter(this.timeProvider.GetToday())
                     {
                         PrincipalName = applyCurrentUser ? withRunAs ? this.actualPrincipalSource.ActualPrincipal.Name : this.userAuthenticationService.GetUserName() : null,
                         SecurityRoleIdents = securityRoleIdents
                     };

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return this.permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
