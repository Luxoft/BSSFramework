using CommonFramework;

using Framework.DomainDriven.Lock;

using SecuritySystem.AccessDenied;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.Repository;

public class GenericRepository<TDomainObject, TIdent>(
    IAsyncDal<TDomainObject, TIdent> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecurityProvider<TDomainObject> securityProvider)
    : IGenericRepository<TDomainObject, TIdent>
    where TDomainObject : class
{
    public async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.CheckAccess(domainObject);

        await dal.SaveAsync(domainObject, cancellationToken);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken)
    {
        this.CheckAccess(domainObject);

        await dal.InsertAsync(domainObject, id, cancellationToken);
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.CheckAccess(domainObject);

        await dal.RemoveAsync(domainObject, cancellationToken);
    }

    private void CheckAccess(TDomainObject domainObject)
    {
        securityProvider.CheckAccess(domainObject, accessDeniedExceptionService);
    }

    public IQueryable<TDomainObject> GetQueryable() => dal.GetQueryable().Pipe(securityProvider.InjectFilter);

    public TDomainObject Load(TIdent id) => dal.Load(id);

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken) =>
        await dal.LoadAsync(id, cancellationToken);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken) =>
        await dal.RefreshAsync(domainObject, cancellationToken);

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
    {
        await dal.LockAsync(domainObject, lockRole, cancellationToken);
    }
}
