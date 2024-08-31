using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    : SecurityFilterBuilder<TPermission, TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public override Expression<Func<TDomainObject, TPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var getIdents = permissionSystem.GetPermissionRestrictions(typeof(TSecurityContext));

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

                                   !getIdents.Eval(permission).Any()

                                   || securityPath.SecurityPathQ.Eval(domainObject)
                                          .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        return (domainObject, permission) =>

                                   !getIdents.Eval(permission).Any()

                                   || securityPath.SecurityPath.Eval(domainObject)
                                          .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                }

            case ManySecurityPathMode.Any:
                {
                    if (securityPath.SecurityPathQ != null)
                    {
                        return (domainObject, permission) =>

                                   !getIdents.Eval(permission).Any()

                                   || !securityPath.SecurityPathQ.Eval(domainObject).Any()

                                   || securityPath.SecurityPathQ.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        return (domainObject, permission) =>

                                   !getIdents.Eval(permission).Any()

                                   || !securityPath.SecurityPath.Eval(domainObject).Any()

                                   || securityPath.SecurityPath.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                }

            default:

                throw new ArgumentOutOfRangeException("securityPath.Mode");
        }
    }
}
