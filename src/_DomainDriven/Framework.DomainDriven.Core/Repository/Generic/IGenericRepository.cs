using Framework.DomainDriven.Lock;

namespace Framework.DomainDriven.Repository;

public interface IGenericRepository<TDomainObject, in TIdent>
{
    Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default);

    Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

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
    Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default);

    Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken = default);
}
