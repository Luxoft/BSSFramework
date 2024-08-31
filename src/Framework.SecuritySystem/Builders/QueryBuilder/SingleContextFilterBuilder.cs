using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var getIdents = permissionSystem.GetPermissionRestrictions(typeof(TSecurityContext));

        var expander = hierarchicalObjectExpanderFactory.CreateQuery(typeof(TSecurityContext));

        var expandExpression = expander.GetExpandExpression(expandType);

        var expandExpressionQ = from idents in getIdents
                                select expandExpression.Eval(idents);

        switch (securityPath.Mode)
        {
            case SingleSecurityMode.AllowNull:

                return (domainObject, permission) =>

                           !getIdents.Eval(permission).Any()

                           || securityPath.SecurityPath.Eval(domainObject) == null

                           || expandExpressionQ.Eval(permission).Contains(securityPath.SecurityPath.Eval(domainObject).Id);

            case SingleSecurityMode.Strictly:

                return (domainObject, permission) =>

                           !getIdents.Eval(permission).Any()

                           || expandExpressionQ.Eval(permission).Contains(securityPath.SecurityPath.Eval(domainObject).Id);

            default:

                throw new ArgumentOutOfRangeException(securityPath.Mode.ToString());
        }
    }
}
