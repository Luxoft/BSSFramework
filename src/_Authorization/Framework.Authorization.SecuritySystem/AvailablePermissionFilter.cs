using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Authorization.Domain;

public class AvailablePermissionFilter
{
    private readonly DateTime today;

    public AvailablePermissionFilter(DateTime today)
    {
        this.today = today;
    }

    public string? PrincipalName { get; set; }

    public List<Guid> SecurityRoleIdents { get; set; }

    public Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return this.GetFilterExpressionElements().BuildAnd();
    }

    public IEnumerable<Expression<Func<Permission, bool>>> GetFilterExpressionElements()
    {
        yield return permission => permission.Status == PermissionStatus.Approved
                                   && permission.Period.Contains(this.today);

        if (this.PrincipalName != null)
        {
            yield return permission => this.PrincipalName == permission.Principal.Name;
        }

        if (!this.SecurityRoleIdents.IsDefault())
        {
            yield return permission => this.SecurityRoleIdents.Contains(permission.Role.Id);
        }
    }
}
