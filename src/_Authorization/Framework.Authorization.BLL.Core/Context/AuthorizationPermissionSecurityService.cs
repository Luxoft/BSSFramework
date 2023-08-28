using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.BLL;

public partial class AuthorizationPermissionSecurityService
{
    public AuthorizationPermissionSecurityService(
            IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            ISecurityOperationResolver<PersistentDomainObjectBase, AuthorizationSecurityOperationCode> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            IAuthorizationBLLContext context)
            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public IAuthorizationBLLContext Context { get; }

    protected override ISecurityProvider<Permission> CreateSecurityProvider(BLLSecurityMode securityMode)
    {
        var baseProvider = base.CreateSecurityProvider(securityMode);

        var withDelegatedFrom = baseProvider.Or(this.Context.GetPrincipalSecurityProvider<Permission>(permission => permission.DelegatedFrom.Principal), this.AccessDeniedExceptionService);

        switch (securityMode)
        {
            case BLLSecurityMode.View:
                return withDelegatedFrom.Or(this.Context.GetPrincipalSecurityProvider<Permission>(permission => permission.Principal), this.AccessDeniedExceptionService);

            case BLLSecurityMode.Edit:
                return withDelegatedFrom;

            default:
                return baseProvider;
        }
    }
}
