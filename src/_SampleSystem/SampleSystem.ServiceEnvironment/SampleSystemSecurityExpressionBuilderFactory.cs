namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityExpressionBuilderFactory<TIdent> : Framework.SecuritySystem.Rules.Builders.Mixed.SecurityExpressionBuilderFactory
{
    public SampleSystemSecurityExpressionBuilderFactory(
        Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<TIdent> hasAccessFactory,
        Framework.SecuritySystem.Rules.Builders.QueryablePermissions.SecurityExpressionBuilderFactory<TIdent> queryFactory)
        : base(hasAccessFactory, queryFactory)
    {
    }
}
