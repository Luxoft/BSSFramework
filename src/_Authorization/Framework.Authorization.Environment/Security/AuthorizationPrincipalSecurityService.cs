using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPrincipalSecurityService : ContextDomainSecurityService<Principal, Guid>
    {
        private readonly IActualPrincipalSource actualPrincipalSource;

        public AuthorizationPrincipalSecurityService(
            ISecurityProvider<Principal> disabledSecurityProvider,
            IEnumerable<ISecurityRuleExpander> securityRuleExpanders,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Principal> securityPath,
            IActualPrincipalSource actualPrincipalSource)
            : base(disabledSecurityProvider, securityRuleExpanders, securityExpressionBuilderFactory, securityPath)
        {
            this.actualPrincipalSource = actualPrincipalSource;
        }

        protected override ISecurityProvider<Principal> CreateSecurityProvider(SecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            if (securityRule == SecurityRule.View)
            {
                return baseProvider.Or(new PrincipalSecurityProvider<Principal>(this.actualPrincipalSource, v => v));
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
