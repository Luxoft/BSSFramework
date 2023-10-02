using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPrincipalSecurityService : ContextDomainSecurityService<Principal, Guid>
    {
        private readonly IRunAsManager runAsManager;

        public AuthorizationPrincipalSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Principal> securityPath,
            IRunAsManager runAsManager)
            : base(disabledSecurityProviderSource, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.runAsManager = runAsManager;
        }

        protected override ISecurityProvider<Principal> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return baseProvider.Or(new PrincipalSecurityProvider<Principal>(this.runAsManager, v => v));

                default:
                    return baseProvider;
            }
        }
    }
}
