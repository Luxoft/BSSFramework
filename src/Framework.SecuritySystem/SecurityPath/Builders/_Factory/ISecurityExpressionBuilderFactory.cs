namespace Framework.SecuritySystem.Rules.Builders;

public interface ISecurityExpressionBuilderFactory
{
    ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject> path);
}
