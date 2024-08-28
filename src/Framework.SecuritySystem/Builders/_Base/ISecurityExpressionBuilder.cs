using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem.Builders._Base;

public interface ISecurityExpressionBuilder<TDomainObject>
{
    ISecurityExpressionFilter<TDomainObject> GetFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<Type> securityTypes);
}
