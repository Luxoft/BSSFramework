#nullable enable

using nuSpec.Abstraction;

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

    /// <summary>
    /// Get Queryable by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    IQueryable<TProjection> GetQueryable<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Single or default by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<TProjection?> SingleOrDefaultAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Single by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<TProjection> SingleAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get First or default by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<TProjection?> FirstOrDefaultAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get First by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<TProjection> FirstAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Count by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<int> CountAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Future query by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    INuFutureEnumerable<TProjection> GetFuture<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Future value by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    INuFutureValue<TProjection> GetFutureValue<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get Future count by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    INuFutureValue<int> GetFutureCount<TProjection>(Specification<TDomainObject, TProjection> specification);

    /// <summary>
    /// Get list by Specification https://github.com/NikitaEgorov/nuSpec
    /// </summary>
    Task<List<TProjection>> GetListAsync<TProjection>(
        Specification<TDomainObject, TProjection> specification,
        CancellationToken cancellationToken = default);
}
