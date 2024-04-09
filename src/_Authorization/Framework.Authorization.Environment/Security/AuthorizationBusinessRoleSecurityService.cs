using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationBusinessRoleSecurityService : ContextDomainSecurityService<BusinessRole, Guid>
    {
        private readonly IAvailablePermissionSource availablePermissionSource;

        public AuthorizationBusinessRoleSecurityService(
            ISecurityProvider<BusinessRole> disabledSecurityProvider,
            ISecurityRuleExpander securityRuleExpander,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<BusinessRole> securityPath,
            IAvailablePermissionSource availablePermissionSource)
            : base(disabledSecurityProvider, securityRuleExpander, securityExpressionBuilderFactory, securityPath)
        {
            this.availablePermissionSource = availablePermissionSource;
        }

        protected override ISecurityProvider<BusinessRole> CreateSecurityProvider(SecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            if (securityRule == SecurityRule.View)
            {
                return baseProvider.Or(new BusinessRoleSecurityProvider<BusinessRole>(this.availablePermissionSource, v => v));
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
