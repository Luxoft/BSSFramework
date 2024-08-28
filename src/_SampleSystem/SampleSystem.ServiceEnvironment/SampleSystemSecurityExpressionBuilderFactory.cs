namespace SampleSystem.ServiceEnvironment;

public class SampleSystemSecurityExpressionBuilderFactory<TIdent>(
    Framework.SecuritySystem.Builders.V1_MaterializedPermissions.SecurityExpressionBuilderFactory hasAccessFactory,
    Framework.SecuritySystem.Builders.V2_QueryablePermissions.SecurityExpressionBuilderFactory queryFactory)
    : Framework.SecuritySystem.Builders.Mixed.SecurityExpressionBuilderFactory(hasAccessFactory, queryFactory);
