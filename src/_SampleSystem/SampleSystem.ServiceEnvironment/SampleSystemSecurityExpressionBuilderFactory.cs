using Framework.Persistent;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> : Framework.SecuritySystem.Rules.Builders.
    Mixed.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent>
    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
{
    public SampleSystemSecurityExpressionBuilderFactory(
        Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> hasAccessFactory,
        Framework.SecuritySystem.Rules.Builders.QueryablePermissions.SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> queryFactory)
        : base(hasAccessFactory, queryFactory)
    {
    }
}
