namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionBuilderFactory : ISecurityExpressionBuilderFactory
{
    private readonly ISecurityExpressionBuilderFactory hasAccessFactory;

    private readonly ISecurityExpressionBuilderFactory queryFactory;

    public SecurityExpressionBuilderFactory(
        ISecurityExpressionBuilderFactory hasAccessFactory,
        ISecurityExpressionBuilderFactory queryFactory)
    {
        this.hasAccessFactory = hasAccessFactory ?? throw new ArgumentNullException(nameof(hasAccessFactory));
        this.queryFactory = queryFactory ?? throw new ArgumentNullException(nameof(queryFactory));
    }

    public ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject> path)
    {
        var hasAccessBuilder = this.hasAccessFactory.CreateBuilder(path);
        var queryBuilder = this.queryFactory.CreateBuilder(path);

        return new SecurityExpressionBuilder<TDomainObject>(hasAccessBuilder, queryBuilder);
    }
}
