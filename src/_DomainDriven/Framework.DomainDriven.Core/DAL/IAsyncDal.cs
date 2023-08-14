namespace Framework.DomainDriven;

public interface IAsyncDal<TDomainObject, in TIdent>
{
    IQueryable<TDomainObject> GetQueryable();

    TDomainObject Load(TIdent id);

    Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task LockAsync(TDomainObject domain, LockRole lockRole, CancellationToken cancellationToken = default);
}
