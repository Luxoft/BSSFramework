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
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Principal> securityPath,
            IActualPrincipalSource actualPrincipalSource)
            : base(disabledSecurityProvider, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.actualPrincipalSource = actualPrincipalSource;
        }

        protected override ISecurityProvider<Principal> CreateSecurityProvider(SecurityRule securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            switch (securityMode)
            {
                case SecurityRule.View:
                    return baseProvider.Or(new PrincipalSecurityProvider<Principal>(this.actualPrincipalSource, v => v));

                default:
                    return baseProvider;
            }
        }
    }
}
