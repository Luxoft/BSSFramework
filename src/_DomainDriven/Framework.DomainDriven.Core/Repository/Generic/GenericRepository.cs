using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.SecuritySystem;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class GenericRepository<TDomainObject, TIdent>(
    IAsyncDal<TDomainObject, TIdent> dal,
    ISpecificationEvaluator specificationEvaluator,
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

    public IQueryable<TProjection> GetQueryable<TProjection>(Specification<TDomainObject, TProjection> specification)
        => specificationEvaluator.GetQuery(this.GetQueryable(), specification);

    public async Task<TProjection?> SingleOrDefaultAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .SingleOrDefaultAsync(cancellationToken);

    public async Task<TProjection> SingleAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .SingleAsync(cancellationToken);

    public async Task<TProjection?> FirstOrDefaultAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .FirstOrDefaultAsync(cancellationToken);

    public async Task<TProjection> FirstAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .FirstAsync(cancellationToken);

    public async Task<int> CountAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .CountAsync(cancellationToken);

    public INuFutureEnumerable<TProjection> GetFuture<TProjection>(
            Specification<TDomainObject, TProjection> specification) =>
            specificationEvaluator.GetFuture(this.GetQueryable(), specification);

    public INuFutureValue<TProjection> GetFutureValue<TProjection>(
            Specification<TDomainObject, TProjection> specification) =>
            specificationEvaluator.GetFutureValue(this.GetQueryable(), specification);

    public INuFutureValue<int> GetFutureCount<TProjection>(Specification<TDomainObject, TProjection> specification) =>
            specificationEvaluator.GetFutureValue(this.GetQueryable(), specification, x => x.Count());

    public async Task<List<TProjection>> GetListAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await specificationEvaluator.GetQuery(this.GetQueryable(), specification).ToListAsync(cancellationToken);

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
    {
        await dal.LockAsync(domainObject, lockRole, cancellationToken);
    }
}
