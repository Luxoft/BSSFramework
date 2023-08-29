using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.Repository;

namespace Framework.Authorization.SecuritySystem;

public class AvailablePermissionSource : IAvailablePermissionSource
{
    private readonly IRepository<Permission> permissionRepository;

    private readonly IDateTimeService dateTimeService;

    private readonly IRunAsManager runAsManager;

    private readonly ICurrentPrincipalSource currentPrincipalSource;

    public AvailablePermissionSource(
        IRepositoryFactory<Permission> permissionRepositoryFactory,
        IDateTimeService dateTimeService,
        IRunAsManager runAsManager,
        ICurrentPrincipalSource currentPrincipalSource)
    {
        this.permissionRepository = permissionRepositoryFactory.Create();
        this.dateTimeService = dateTimeService;
        this.runAsManager = runAsManager;
        this.currentPrincipalSource = currentPrincipalSource;
    }

    public IQueryable<Permission> GetAvailablePermissionsQueryable(bool withRunAs = true)
    {
        return this.permissionRepository.GetQueryable().Where(this.GetFilterExpression(withRunAs));
    }

    private Expression<Func<Permission, bool>> GetFilterExpression(bool withRunAs = true)
    {
        var principalName = withRunAs ? this.runAsManager.PrincipalName : this.currentPrincipalSource.CurrentPrincipal.Name;

        var dateTime = this.dateTimeService.Today;

        return permission => principalName == permission.Principal.Name
                             && permission.Status == PermissionStatus.Approved
                             && permission.Period.Contains(dateTime);
    }
}
