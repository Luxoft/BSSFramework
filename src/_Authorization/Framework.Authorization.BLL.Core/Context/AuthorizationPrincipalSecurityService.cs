using Framework.Authorization.Domain;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.BLL
{
    public class AuthorizationPrincipalSecurityService : ContextDomainSecurityService<Principal, Guid>
    {
        public AuthorizationPrincipalSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Principal> securityPath,
            IAuthorizationBLLContext context)
            : base(disabledSecurityProviderSource, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAuthorizationBLLContext Context { get; }

        protected override ISecurityProvider<Principal> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return this.Context.GetPrincipalSecurityProvider().Or(baseProvider);

                default:
                    return baseProvider;
            }
        }
    }
}
