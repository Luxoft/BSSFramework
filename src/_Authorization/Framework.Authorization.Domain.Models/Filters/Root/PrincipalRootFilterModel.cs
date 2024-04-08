using System.Linq.Expressions;

namespace Framework.Authorization.Domain;

public class PrincipalRootFilterModel : DomainObjectRootFilterModel<Principal>
{
    public BusinessRole BusinessRole { get; set; }

    public override Expression<Func<Principal, bool>> ToFilterExpression()
    {
        var businessRole = this.BusinessRole;

        return principal => businessRole == null || principal.Permissions.Any(permission => permission.Role == businessRole);
    }
}
