using System.Linq.Expressions;
using Framework.Core;

namespace Framework.Authorization.Domain;

public class AvailablePermissionFilter(DateTime today)
{
    public string? PrincipalName { get; set; }

    public List<Guid>? SecurityRoleIdents { get; set; }

    public Dictionary<Guid, (bool, Expression<Func<Guid, bool>>)> RestrictionFilters { get; set; } = new();

    public Expression<Func<Permission, bool>> ToFilterExpression()
    {
        return this.GetFilterExpressionElements().BuildAnd();
    }

    public IEnumerable<Expression<Func<Permission, bool>>> GetFilterExpressionElements()
    {
        yield return permission => permission.Period.Contains(today);

        if (this.PrincipalName != null)
        {
            yield return permission => this.PrincipalName == permission.Principal.Name;
        }

        if (this.SecurityRoleIdents != null)
        {
            yield return permission => this.SecurityRoleIdents.Contains(permission.Role.Id);
        }

        foreach (var (securityContextTypeId, (allowGrandAccess, restrictionFilterExpr)) in this.RestrictionFilters)
        {
            var baseFilter =
                ExpressionHelper
                    .Create(
                        (Permission permission) => permission.Restrictions.Any(
                            r => r.SecurityContextType.Id == securityContextTypeId && restrictionFilterExpr.Eval(r.SecurityContextId)))
                    .ExpandConst().InlineEval();

            if (allowGrandAccess)
            {
                var grandAccessExpr = ExpressionHelper.Create(
                    (Permission permission) =>
                        permission.Restrictions.All(r => r.SecurityContextType.Id != securityContextTypeId));

                yield return baseFilter.BuildOr(grandAccessExpr);
            }
            else
            {
                yield return baseFilter;
            }
        }
    }
}
