namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionBuilder<TDomainObject> : ISecurityExpressionBuilder<TDomainObject>
{
    private readonly ISecurityExpressionBuilder<TDomainObject> hasAccessBuilder;

    private readonly ISecurityExpressionBuilder<TDomainObject> queryBuilder;

    public SecurityExpressionBuilder(
        ISecurityExpressionBuilder<TDomainObject> hasAccessBuilder,
        ISecurityExpressionBuilder<TDomainObject> queryBuilder)
    {
        this.hasAccessBuilder = hasAccessBuilder ?? throw new ArgumentNullException(nameof(hasAccessBuilder));
        this.queryBuilder = queryBuilder ?? throw new ArgumentNullException(nameof(queryBuilder));
    }

    public ISecurityExpressionFilter<TDomainObject> GetFilter(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        var hasAccessFilter = this.hasAccessBuilder.GetFilter(securityRule);
        var queryFilter = this.queryBuilder.GetFilter(securityRule);

        return new SecurityExpressionFilter<TDomainObject>(hasAccessFilter, queryFilter);
    }
}
