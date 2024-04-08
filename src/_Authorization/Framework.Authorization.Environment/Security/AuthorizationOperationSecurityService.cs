using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationOperationSecurityService : ContextDomainSecurityService<Operation, Guid>
    {
        private readonly IAvailablePermissionSource availablePermissionSource;

        public AuthorizationOperationSecurityService(
            ISecurityProvider<Operation> disabledSecurityProvider,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Operation> securityPath,
            IAvailablePermissionSource availablePermissionSource)
            : base(disabledSecurityProvider, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.availablePermissionSource = availablePermissionSource;
        }

        protected override ISecurityProvider<Operation> CreateSecurityProvider(SecurityRule securityRule)
        {
            var baseProvider = base.CreateSecurityProvider(securityRule);

            switch (securityRule)
            {
                case SecurityRule.View:
                    return baseProvider.Or(new OperationSecurityProvider(this.availablePermissionSource));

                default:
                    return baseProvider;
            }
        }
    }
}
