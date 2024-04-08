using System.Linq.Expressions;

namespace Framework.Authorization.Domain;

public class BusinessRoleRootFilterModel : DomainObjectRootFilterModel<BusinessRole>
{
    public Principal Principal { get; set; }

    public override Expression<Func<BusinessRole, bool>> ToFilterExpression()
    {
        var principal = this.Principal;

        return businessRole => (principal == null || businessRole.Permissions.Any(permission => permission.Principal == principal));
    }
}
