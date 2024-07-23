namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilder<TDomainObject>
{
    ISecurityExpressionFilter<TDomainObject> GetFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<Type> securityTypes);
}
