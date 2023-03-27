using System.Linq.Expressions;

namespace Framework.Authorization.Domain;

public class OperationRootFilterModel : DomainObjectRootFilterModel<Operation>
{
    public Principal Principal { get; set; }

    public BusinessRole BusinessRole { get; set; }

    public override Expression<Func<Operation, bool>> ToFilterExpression()
    {
        var businessRole = this.BusinessRole;

        var principal = this.Principal;

        return operation => (businessRole == null || operation.Links.Any(link => link.BusinessRole == businessRole))
                            && (principal == null || operation.Links.Any(link => link.BusinessRole.Permissions.Any(permission => permission.Principal == principal)));
    }
}
