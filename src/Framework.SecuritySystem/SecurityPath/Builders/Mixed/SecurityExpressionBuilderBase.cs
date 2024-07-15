namespace Framework.SecuritySystem.Rules.Builders.Mixed;

public class SecurityExpressionBuilder<TDomainObject>(
    ISecurityExpressionBuilder<TDomainObject> hasAccessBuilder,
    ISecurityExpressionBuilder<TDomainObject> queryBuilder)
    : ISecurityExpressionBuilder<TDomainObject>
{
    public ISecurityExpressionFilter<TDomainObject> GetFilter(
        SecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var cachedSecurityTypes = securityTypes.ToList();

        var hasAccessFilter = hasAccessBuilder.GetFilter(securityRule, cachedSecurityTypes);
        var queryFilter = queryBuilder.GetFilter(securityRule, cachedSecurityTypes);

        return new SecurityExpressionFilter<TDomainObject>(hasAccessFilter, queryFilter);
    }
}
