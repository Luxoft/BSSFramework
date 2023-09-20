using Framework.Persistent;
using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class UntypedDependencyDomainSecurityService<TDomainObject, TBaseDomainObject, TIdent> :

    DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>

    where TDomainObject : IIdentityObject<TIdent>
    where TBaseDomainObject : class, IIdentityObject<TIdent>
{
    private readonly IQueryableSource queryableSource;

    public UntypedDependencyDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
        IQueryableSource queryableSource)

        : base(disabledSecurityProviderSource, securityOperationResolver, baseDomainSecurityService)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
    }

    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new UntypedDependencySecurityProvider<TDomainObject, TBaseDomainObject, TIdent>(
            baseProvider,
            this.queryableSource);
    }
}
