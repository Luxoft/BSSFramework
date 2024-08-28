using Framework.SecuritySystem.Builders._Base;

namespace Framework.SecuritySystem.Builders._Factory;

public interface ISecurityExpressionBuilderFactory
{
    ISecurityExpressionBuilder<TDomainObject> CreateBuilder<TDomainObject>(SecurityPath<TDomainObject> path);
}
