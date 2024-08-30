using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SingleContextFilterBuilder<TDomainObject, TSecurityContext>(
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    ISecurityContextSource securityContextSource,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    : SecurityFilterBuilder<TDomainObject>
    where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
{
    public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
    {
        var securityContextTypeId = securityContextSource.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

        var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

        var getIdents = ExpressionHelper.Create(
                                            (IPermission permission) =>
                                                permission.Restrictions
                                                          .Where(
                                                              item => eqIdentsExpr.Eval(
                                                                  item.SecurityContextTypeId,
                                                                  securityContextTypeId))
                                                          .Select(fi => fi.SecurityContextId))
                                        .ExpandConst()
                                        .InlineEval();

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
