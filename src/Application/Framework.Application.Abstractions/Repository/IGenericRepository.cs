using Framework.Database;

namespace Framework.Application.Repository;

public interface IGenericRepository<TDomainObject, in TIdent>
{
    Task SaveAsync(TDomainObject domainObject, CancellationToken ct);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken ct);

    IQueryable<TDomainObject> GetQueryable();

    /// <summary>
    /// Load actually returns a proxy object and doesn't need to access the database right when you issue that Load call.
    /// https://www.tutorialspoint.com/nhibernate/nhibernate_load_get.htm
    /// </summary>
    TDomainObject Load(TIdent id);

    /// <summary>
    /// Load actually returns a proxy object and doesn't need to access the database right when you issue that Load call.
    /// https://www.tutorialspoint.com/nhibernate/nhibernate_load_get.htm
    /// </summary>
    Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    Task RefreshAsync(TDomainObject domainObject, CancellationToken ct);

    Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct);
}

