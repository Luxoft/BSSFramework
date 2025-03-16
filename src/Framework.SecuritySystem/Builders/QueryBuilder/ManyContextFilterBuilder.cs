using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
    SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var grandAccessExpr = permissionSystem.GetGrandAccessExpr<TSecurityContext>();

        var getIdents = permissionSystem.GetPermissionRestrictionsExpr(restrictionFilterInfo);

        var expander =
            (IHierarchicalObjectQueryableExpander<Guid>)hierarchicalObjectExpanderFactory.Create(
                typeof(TSecurityContext));

        var expandExpression = expander.GetExpandExpression(expandType);

        var expandExpressionQ = from idents in getIdents

                                select expandExpression.Eval(idents);

        switch (securityPath.Mode)
        {
            case ManySecurityPathMode.AnyStrictly:
                {
                    if (securityPath.SecurityPathQ != null)
                    {
                        return (domainObject, permission) =>

                                   grandAccessExpr.Eval(permission)

                                   || securityPath.SecurityPathQ.Eval(domainObject)
                                          .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        return (domainObject, permission) =>

                                   grandAccessExpr.Eval(permission)

                                   || securityPath.Expression.Eval(domainObject)
                                          .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                }

            case ManySecurityPathMode.Any:
                {
                    if (securityPath.SecurityPathQ != null)
                    {
                        return (domainObject, permission) =>

                                   grandAccessExpr.Eval(permission)

                                   || !securityPath.SecurityPathQ.Eval(domainObject).Any()

                                   || securityPath.SecurityPathQ.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        return (domainObject, permission) =>

                                   grandAccessExpr.Eval(permission)

                                   || !securityPath.Expression.Eval(domainObject).Any()

                                   || securityPath.Expression.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                }

            default:

                throw new ArgumentOutOfRangeException("securityPath.Mode");
        }
    }
}
