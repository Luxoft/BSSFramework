using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.Environment
{
    public class AuthorizationPermissionSecurityService : ContextDomainSecurityService<Permission, Guid>
    {
        private readonly IRunAsManager runAsManager;

        public AuthorizationPermissionSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Permission> securityPath,
            IRunAsManager runAsManager)
            : base(disabledSecurityProviderSource, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.runAsManager = runAsManager;
        }

        protected override ISecurityProvider<Permission> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            var withDelegatedFrom = baseProvider.Or(
                new PrincipalSecurityProvider<Permission>(this.runAsManager, permission => permission.DelegatedFrom.Principal));

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return withDelegatedFrom.Or(
                        new PrincipalSecurityProvider<Permission>(this.runAsManager, permission => permission.Principal));

                case BLLSecurityMode.Edit:
                    return withDelegatedFrom;

                default:
                    return baseProvider;
            }
        }
    }
}
