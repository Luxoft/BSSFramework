namespace Framework.Database;

public interface IAsyncDal<TDomainObject, in TIdent>
{
    IQueryable<TDomainObject> GetQueryable();

    TDomainObject Load(TIdent id);

    Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    Task RefreshAsync(TDomainObject domainObject, CancellationToken ct);

    Task SaveAsync(TDomainObject domainObject, CancellationToken ct);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken ct);

    Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct);
}
