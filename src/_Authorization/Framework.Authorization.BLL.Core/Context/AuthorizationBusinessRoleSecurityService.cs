using Framework.Authorization.Domain;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.BLL
{
    public class AuthorizationBusinessRoleSecurityService : ContextDomainSecurityService<BusinessRole, Guid>
    {
        public AuthorizationBusinessRoleSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<BusinessRole> securityPath,
            IAuthorizationBLLContext context)
            : base(disabledSecurityProviderSource, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAuthorizationBLLContext Context { get; }

        protected override ISecurityProvider<BusinessRole> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return this.Context.GetBusinessRoleSecurityProvider().Or(baseProvider);

                default:
                    return baseProvider;
            }
        }
    }
}
