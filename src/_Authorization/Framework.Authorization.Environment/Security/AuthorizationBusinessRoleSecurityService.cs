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
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<BusinessRole> securityPath,
            IAvailablePermissionSource availablePermissionSource)
            : base(disabledSecurityProvider, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.availablePermissionSource = availablePermissionSource;
        }

        protected override ISecurityProvider<BusinessRole> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return baseProvider.Or(new BusinessRoleSecurityProvider<BusinessRole>(this.availablePermissionSource, v => v));

                default:
                    return baseProvider;
            }
        }
    }
}
