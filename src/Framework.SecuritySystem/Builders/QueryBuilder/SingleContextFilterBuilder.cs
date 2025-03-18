using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
    SecurityContextRestriction<TSecurityContext>? securityContextRestriction)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var allowGrandAccess = securityContextRestriction?.Required != true;

        var grandAccessExpr = allowGrandAccess
                                  ? permissionSystem.GetGrandAccessExpr<TSecurityContext>()
                                  : _ => false;

        var getIdents = permissionSystem.GetPermissionRestrictionsExpr(securityContextRestriction?.Filter);

        var expander = hierarchicalObjectExpanderFactory.CreateQuery(typeof(TSecurityContext));

        var expandExpression = expander.GetExpandExpression(expandType);

        var expandExpressionQ = from idents in getIdents

                                select expandExpression.Eval(idents);

        if (securityPath.Required)
        {
            return (domainObject, permission) =>

                       grandAccessExpr.Eval(permission)

                       || securityPath.Expression.Eval(domainObject) == null

                       || expandExpressionQ.Eval(permission).Contains(securityPath.Expression.Eval(domainObject).Id);
        }
        else
        {
            return (domainObject, permission) =>

                       grandAccessExpr.Eval(permission)

                       || expandExpressionQ.Eval(permission).Contains(securityPath.Expression.Eval(domainObject).Id);
        }
    }
}
