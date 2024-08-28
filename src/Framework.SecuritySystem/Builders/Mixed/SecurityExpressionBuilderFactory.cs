using Framework.SecuritySystem.Builders._Base;
using Framework.SecuritySystem.Builders._Factory;

namespace Framework.SecuritySystem.Builders.Mixed;

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
