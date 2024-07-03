using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

namespace Framework.Authorization.Environment
{
    public class AuthorizationBusinessRoleSecurityService(
        IServiceProvider serviceProvider,
        ISecurityProvider<BusinessRole> disabledSecurityProvider,
        ISecurityRuleExpander securityRuleExpander,
        ISecurityPathProviderFactory securityPathProviderFactory,
        SecurityPath<BusinessRole> securityPath,
        IAvailablePermissionSource availablePermissionSource)
        : ContextDomainSecurityService<BusinessRole>(
            serviceProvider,
            disabledSecurityProvider,
            securityRuleExpander,
            securityPathProviderFactory,
            securityPath)
    {
        protected override ISecurityProvider<BusinessRole> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            if (securityRule == SecurityRule.View)
            {
                return baseProvider.Or(new BusinessRoleSecurityProvider<BusinessRole>(availablePermissionSource, v => v));
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
