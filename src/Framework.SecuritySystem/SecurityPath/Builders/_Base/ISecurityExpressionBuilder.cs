namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilder<TDomainObject>
{
    ISecurityExpressionFilter<TDomainObject> GetFilter(SecurityRule.DomainObjectSecurityRule securityRule);
}
