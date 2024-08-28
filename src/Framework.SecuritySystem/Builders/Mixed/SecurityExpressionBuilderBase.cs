using Framework.SecuritySystem.Builders._Base;
using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem.Builders.Mixed;

public class SecurityExpressionBuilder<TDomainObject>(
    ISecurityExpressionBuilder<TDomainObject> hasAccessBuilder,
    ISecurityExpressionBuilder<TDomainObject> queryBuilder)
    : ISecurityExpressionBuilder<TDomainObject>
{
    public ISecurityExpressionFilter<TDomainObject> GetFilter(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var cachedSecurityTypes = securityTypes.ToList();

        var hasAccessFilter = hasAccessBuilder.GetFilter(securityRule, cachedSecurityTypes);
        var queryFilter = queryBuilder.GetFilter(securityRule, cachedSecurityTypes);

        return new SecurityExpressionFilter<TDomainObject>(hasAccessFilter, queryFilter);
    }
}
