using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPermissionSecurityService : ContextDomainSecurityService<Permission, Guid>
    {
        private readonly IActualPrincipalSource actualPrincipalSource;

        public AuthorizationPermissionSecurityService(
            ISecurityProvider<Permission> disabledSecurityProvider,
            ISecurityRuleExpander securityRuleExpander,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Permission> securityPath,
            IActualPrincipalSource actualPrincipalSource)
            : base(disabledSecurityProvider, securityRuleExpander, securityExpressionBuilderFactory, securityPath)
        {
            this.actualPrincipalSource = actualPrincipalSource;
        }

        protected override ISecurityProvider<Permission> CreateSecurityProvider(SecurityRule.SpecialSecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            var withDelegatedFrom = baseProvider.Or(
                new PrincipalSecurityProvider<Permission>(this.actualPrincipalSource, permission => permission.DelegatedFrom.Principal));

            if (securityRule == SecurityRule.View)
            {
                return withDelegatedFrom.Or(
                    new PrincipalSecurityProvider<Permission>(this.actualPrincipalSource, permission => permission.Principal));
            }
            else if (securityRule == SecurityRule.Edit)
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
