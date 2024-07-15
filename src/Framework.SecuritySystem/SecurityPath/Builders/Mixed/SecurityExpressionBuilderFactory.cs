namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionBuilderFactory(
    ISecurityExpressionBuilderFactory hasAccessFactory,
    ISecurityExpressionBuilderFactory queryFactory)
    : ISecurityExpressionBuilderFactory
{
    public ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject> path)
    {
        var hasAccessBuilder = hasAccessFactory.CreateBuilder(path);

        var queryBuilder = queryFactory.CreateBuilder(path);

        return new SecurityExpressionBuilder<TDomainObject>(hasAccessBuilder, queryBuilder);
    }
}
