using System.Linq;
using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource : IAvailablePermissionSource
{
    private readonly IRepository<Permission> permissionRepository;

    private readonly IDateTimeService dateTimeService;

    private readonly IRunAsManager runAsManager;

    private readonly IUserAuthenticationService userAuthenticationService;

    public AvailablePermissionSource(
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IDateTimeService dateTimeService,
        IRunAsManager runAsManager,
        IUserAuthenticationService userAuthenticationService)
    {
        this.permissionRepository = permissionRepositoryFactory.Create();
        this.dateTimeService = dateTimeService;
        this.runAsManager = runAsManager;
        this.userAuthenticationService = userAuthenticationService;
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true, Guid securityOperationId = default)
    {
        var filter = new AvailablePermissionFilter(this.dateTimeService.Today)
                     {
                         PrincipalName = withRunAs ? this.runAsManager.PrincipalName : this.userAuthenticationService.GetUserName(),
                         SecurityOperationId = securityOperationId
                     };

        return this.GetAvailablePermissionsQueryable(filter);
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(AvailablePermissionFilter filter)
    {
        return this.permissionRepository.GetQueryable().Where(filter.ToFilterExpression());
    }
}
