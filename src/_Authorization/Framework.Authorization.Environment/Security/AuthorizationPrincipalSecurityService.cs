using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPrincipalSecurityService(
        ISecurityProvider<Principal> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        ISecurityPathProviderFactory securityPathProviderFactory,
        SecurityPath<Principal> securityPath,
        IActualPrincipalSource actualPrincipalSource)
        : ContextDomainSecurityService<Principal>(disabledSecurityProvider, securityRuleExpander, securityPathProviderFactory, securityPath)
    {
        protected override ISecurityProvider<Principal> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            if (securityRule == SecurityRule.View)
            {
                return baseProvider.Or(new PrincipalSecurityProvider<Principal>(actualPrincipalSource, v => v));
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
