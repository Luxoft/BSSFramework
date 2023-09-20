using Framework.QueryableSource;

namespace Framework.SecuritySystem;

public class DependencyDomainSecurityService<TDomainObject, TBaseDomainObject> :

    DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>
{
    private readonly IQueryableSource queryableSource;

    private readonly DependencyDomainSecurityServicePath<TDomainObject, TBaseDomainObject> path;

    public DependencyDomainSecurityService(
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityOperationResolver securityOperationResolver,
        IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
        IQueryableSource queryableSource,
        DependencyDomainSecurityServicePath<TDomainObject, TBaseDomainObject> path)

        : base(disabledSecurityProviderSource, securityOperationResolver, baseDomainSecurityService)
    {
        this.queryableSource = queryableSource ?? throw new ArgumentNullException(nameof(queryableSource));
        this.path = path;
    }

    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
            baseProvider,
            this.path.Selector,
            this.queryableSource);
    }
}
