using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven;

namespace Framework.Authorization.Domain;

public class AvailablePermissionOperationFilter : AvailablePermissionFilter
{
    private readonly Operation operation;

    public AvailablePermissionOperationFilter(IDateTimeService dateTimeService, string principalName, Operation operation)
            : base(dateTimeService, principalName)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));

        this.operation = operation;
    }

    public override Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return base.ToFilterExpression()
                   .BuildAnd(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation == this.operation));
    }
}
