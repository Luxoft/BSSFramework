namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilder<TDomainObject>
{
    ISecurityExpressionFilter<TDomainObject> GetFilter(ContextSecurityOperation securityOperation);
}
