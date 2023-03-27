using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;

using JetBrains.Annotations;

namespace Framework.Authorization.Domain;

public class AvailablePermissionOperationFilter<TSecurityOperationCode> : AvailablePermissionFilter
        where TSecurityOperationCode : struct, Enum
{
    private readonly Guid securityOperationId;

    public AvailablePermissionOperationFilter([NotNull] IDateTimeService dateTimeService, string principalName, TSecurityOperationCode securityOperationCode)
            : base(dateTimeService, principalName)
    {
        if (securityOperationCode.IsDefault()) throw new ArgumentOutOfRangeException(nameof(securityOperationCode));

        this.securityOperationId = securityOperationCode.ToGuid();
    }

    public override Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return base.ToFilterExpression()
                   .BuildAnd(permission => permission.Role.BusinessRoleOperationLinks.Any(link => link.Operation.Id == this.securityOperationId));
    }
}
