using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL
{
    public partial class AuthorizationBusinessRoleSecurityService
    {
        public AuthorizationBusinessRoleSecurityService(
            IDisabledSecurityProviderSource<PersistentDomainObjectBase> disabledSecurityProviderSource,
            ISecurityOperationResolver<PersistentDomainObjectBase, AuthorizationSecurityOperationCode> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            IAuthorizationBLLContext context)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem)
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
                    return this.Context.GetBusinessRoleSecurityProvider().Or(baseProvider, this.AccessDeniedExceptionService);

                default:
                    return baseProvider;
            }
        }
    }
}
