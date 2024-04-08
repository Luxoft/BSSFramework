using Framework.SecuritySystem.Rules.Builders;
using Framework.SecuritySystem.Providers.Operation;

namespace Framework.SecuritySystem;

public static class SecurityPathExtensions
{
    public static ISecurityProvider<TDomainObject> ToProvider<TDomainObject>(
        this SecurityPath<TDomainObject> securityPath,
        SecurityRule.RolesSecurityRule securityRule ,
        ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPath,
            securityRule,
            securityExpressionBuilderFactory);
    }
}
