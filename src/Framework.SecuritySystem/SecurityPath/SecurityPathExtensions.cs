using Framework.SecuritySystem.Rules.Builders;
using Framework.SecuritySystem.Providers.Operation;

namespace Framework.SecuritySystem;

public static class SecurityPathExtensions
{
    public static ISecurityProvider<TDomainObject> ToProvider<TDomainObject>(
        this SecurityPath<TDomainObject> securityPath,
        SecurityOperation operation,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPath,
            operation,
            securityExpressionBuilderFactory);
    }
}
