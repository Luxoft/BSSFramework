using Framework.Persistent;

namespace Framework.SecuritySystem;

/// <summary>
/// Сервис с кешированием доступа к неконтекстным операциям
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TDomainObject"></typeparam>
/// <typeparam name="TIdent"></typeparam>
public abstract class NonContextDomainSecurityService<TPersistentDomainObjectBase, TDomainObject, TIdent> : DomainSecurityService<TPersistentDomainObjectBase, TDomainObject>

    where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
    where TDomainObject : class, TPersistentDomainObjectBase
{
    private readonly IDisabledSecurityProviderSource disabledSecurityProviderSource;

    private readonly IAuthorizationSystem<TIdent> authorizationSystem;


    protected NonContextDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IAuthorizationSystem<TIdent> authorizationSystem)
        : base(disabledSecurityProviderSource, securityOperationResolver)
    {
        this.disabledSecurityProviderSource = disabledSecurityProviderSource;
        this.authorizationSystem = authorizationSystem ?? throw new ArgumentNullException(nameof(authorizationSystem));
    }

    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(SecurityOperation securityOperation)
    {
        if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

        switch (securityOperation)
        {
            case NonContextSecurityOperation nonContextSecurityOperation:
                return this.CreateSecurityProvider(nonContextSecurityOperation);

            case DisabledSecurityOperation:
                return this.disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();

            default:
                throw new InvalidOperationException($"invalid operation: {securityOperation}");
        }
    }

    protected virtual ISecurityProvider<TDomainObject> CreateSecurityProvider(NonContextSecurityOperation securityOperation)
    {
        return new NonContextSecurityProvider<TPersistentDomainObjectBase, TDomainObject, TIdent>(securityOperation, this.authorizationSystem);
    }
}
