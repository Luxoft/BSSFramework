using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPermissionSecurityService(
        ISecurityProvider<Permission> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        ISecurityPathProviderFactory securityPathProviderFactory,
        SecurityPath<Permission> securityPath,
        IActualPrincipalSource actualPrincipalSource)
        : ContextDomainSecurityService<Permission>(
            disabledSecurityProvider,
            securityRuleExpander,
            securityPathProviderFactory,
            securityPath)
    {
        protected override ISecurityProvider<Permission> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            var withDelegatedFrom = baseProvider.Or(
                new PrincipalSecurityProvider<Permission>(actualPrincipalSource, permission => permission.DelegatedFrom.Principal));

            if (securityRule == SecurityRule.View)
            {
                return withDelegatedFrom.Or(
                    new PrincipalSecurityProvider<Permission>(actualPrincipalSource, permission => permission.Principal));
            }
            else if (securityRule == SecurityRule.Edit || securityRule == SecurityRule.Remove)
            {
                return withDelegatedFrom;
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
