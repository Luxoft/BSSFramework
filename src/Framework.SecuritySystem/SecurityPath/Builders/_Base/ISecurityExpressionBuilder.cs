namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilder<TDomainObject>
{
    ISecurityExpressionFilter<TDomainObject> GetFilter(SecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<Type> securityTypes);
}
