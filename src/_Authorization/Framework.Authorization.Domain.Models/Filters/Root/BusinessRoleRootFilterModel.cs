using System;
using System.Linq;
using System.Linq.Expressions;

namespace Framework.Authorization.Domain;

public class BusinessRoleRootFilterModel : DomainObjectRootFilterModel<BusinessRole>
{
    public Operation Operation { get; set; }

    public Principal Principal { get; set; }

    public override Expression<Func<BusinessRole, bool>> ToFilterExpression()
    {
        var operation = this.Operation;

        var principal = this.Principal;

        return businessRole => (operation == null || businessRole.BusinessRoleOperationLinks.Any(link => link.Operation == operation))
                               && (principal == null || businessRole.Permissions.Any(permission => permission.Principal == principal));
    }
}
