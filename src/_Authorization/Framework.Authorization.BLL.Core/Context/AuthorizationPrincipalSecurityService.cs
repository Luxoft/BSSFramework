using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL
{
    public partial class AuthorizationPrincipalSecurityService
    {
        public AuthorizationPrincipalSecurityService(
            IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            ISecurityOperationResolver<PersistentDomainObjectBase, AuthorizationSecurityOperationCode> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            IAuthorizationBLLContext context)
            : base(disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
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
