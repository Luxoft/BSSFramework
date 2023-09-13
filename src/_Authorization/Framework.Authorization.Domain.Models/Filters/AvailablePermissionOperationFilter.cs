using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven;

namespace Framework.Authorization.Domain;

public class AvailablePermissionOperationFilter : AvailablePermissionFilter
{
    private readonly Guid securityOperationId;

    public AvailablePermissionOperationFilter(IDateTimeService dateTimeService, string principalName, Operation operation)
        : this(dateTimeService, principalName, operation.Id)
    {
    }

    public AvailablePermissionOperationFilter(IDateTimeService dateTimeService, string principalName, Guid securityOperationId)
            : base(dateTimeService, principalName)
    {
        this.securityOperationId = securityOperationId;
    }

    public override Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return base.ToFilterExpression()
                   .BuildAnd(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation.Id == this.securityOperationId));
    }
}
