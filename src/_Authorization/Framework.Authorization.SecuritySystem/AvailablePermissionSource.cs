﻿using Framework.Authorization.Domain;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource : IAvailablePermissionSource
{
    private readonly IRepository<Permission> permissionRepository;

    private readonly IDateTimeService dateTimeService;

    private readonly IActualPrincipalSource actualPrincipalSource;

    private readonly IUserAuthenticationService userAuthenticationService;

    public AvailablePermissionSource(
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IDateTimeService dateTimeService,
        IActualPrincipalSource actualPrincipalSource,
        IUserAuthenticationService userAuthenticationService)
    {
        this.permissionRepository = permissionRepositoryFactory.Create();
        this.dateTimeService = dateTimeService;
        this.actualPrincipalSource = actualPrincipalSource;
        this.userAuthenticationService = userAuthenticationService;
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, Guid securityOperationId = default, bool applyCurrentUser = true)
    {
        var filter = new AvailablePermissionFilter(this.dateTimeService.Today)
                     {
                         PrincipalName = applyCurrentUser ? withRunAs ? this.actualPrincipalSource.ActualPrincipal.Name : this.userAuthenticationService.GetUserName() : null,
                         SecurityOperationId = securityOperationId
                     };

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return this.permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
