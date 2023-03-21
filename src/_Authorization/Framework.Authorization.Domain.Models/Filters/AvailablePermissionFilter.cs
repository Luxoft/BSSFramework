using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven;

using JetBrains.Annotations;

namespace Framework.Authorization.Domain;

public class AvailablePermissionFilter : IDomainObjectFilterModel<Permission>
{
    private readonly IDateTimeService dateTimeService;

    private readonly string principalName;

    public AvailablePermissionFilter([NotNull] IDateTimeService dateTimeService, string principalName)
    {
        if (dateTimeService == null) throw new ArgumentNullException(nameof(dateTimeService));

        this.dateTimeService = dateTimeService;
        this.principalName = principalName;
    }

    public virtual Expression<Func<Permission, bool>> ToFilterExpression()
    {
        var dateTime = this.dateTimeService.Today;

        return permission => (this.principalName == null || this.principalName == permission.Principal.Name)
                             && permission.Status == PermissionStatus.Approved
                             && permission.Period.Contains(dateTime);
    }
}
