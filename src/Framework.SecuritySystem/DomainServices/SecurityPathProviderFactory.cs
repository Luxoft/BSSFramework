using Framework.SecuritySystem.Providers.Operation;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem;

public class SecurityPathProviderFactory(ISecurityExpressionBuilderFactory securityExpressionBuilderFactory) : ISecurityPathProviderFactory
{
    public ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPath,
            securityRule,
            securityExpressionBuilderFactory);
    }
}
