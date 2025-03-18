using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
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

        var expander =
            (IHierarchicalObjectQueryableExpander<Guid>)hierarchicalObjectExpanderFactory.Create(
                typeof(TSecurityContext));

        var expandExpression = expander.GetExpandExpression(expandType);

        var expandExpressionQ = from idents in getIdents

                                select expandExpression.Eval(idents);

        if (securityPath.Required)
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
        else
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
    }
}
