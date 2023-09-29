using Framework.Authorization.Domain;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.Authorization.BLL
{
    public class AuthorizationPermissionSecurityService : ContextDomainSecurityService<Permission, Guid>
    {
        public AuthorizationPermissionSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Permission> securityPath,
            IAuthorizationBLLContext context)
            : base(disabledSecurityProviderSource, securityOperationResolver, securityExpressionBuilderFactory, securityPath)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IAuthorizationBLLContext Context { get; }

        protected override ISecurityProvider<Permission> CreateSecurityProvider(BLLSecurityMode securityMode)
        {
            var baseProvider = base.CreateSecurityProvider(securityMode);

            var withDelegatedFrom = baseProvider.Or(this.Context.GetPrincipalSecurityProvider<Permission>(permission => permission.DelegatedFrom.Principal));

            switch (securityMode)
            {
                case BLLSecurityMode.View:
                    return withDelegatedFrom.Or(this.Context.GetPrincipalSecurityProvider<Permission>(permission => permission.Principal));

                case BLLSecurityMode.Edit:
                    return withDelegatedFrom;

                default:
                    return baseProvider;
            }
        }
    }
}
