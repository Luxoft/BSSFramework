using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
    SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var grandAccessExpr = permissionSystem.GetGrandAccessExpr<TSecurityContext>();

        var getIdents = permissionSystem.GetPermissionRestrictionsExpr(restrictionFilterInfo);

        var expander = hierarchicalObjectExpanderFactory.CreateQuery(typeof(TSecurityContext));

        var expandExpression = expander.GetExpandExpression(expandType);

        var expandExpressionQ = from idents in getIdents

                                select expandExpression.Eval(idents);

        switch (securityPath.Mode)
        {
            case SingleSecurityMode.AllowNull:

                return (domainObject, permission) =>

                           grandAccessExpr.Eval(permission)

                           || securityPath.SecurityPath.Eval(domainObject) == null

                           || expandExpressionQ.Eval(permission).Contains(securityPath.SecurityPath.Eval(domainObject).Id);

            case SingleSecurityMode.Strictly:

                return (domainObject, permission) =>

                           grandAccessExpr.Eval(permission)

                           || expandExpressionQ.Eval(permission).Contains(securityPath.SecurityPath.Eval(domainObject).Id);

            default:

                throw new ArgumentOutOfRangeException(securityPath.Mode.ToString());
        }
    }
}
