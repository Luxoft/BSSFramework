#nullable enable

using Framework.Core;
using Framework.SecuritySystem;

using NHibernate.Linq;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class GenericRepository<TDomainObject, TIdent> : IGenericRepository<TDomainObject, TIdent>
        where TDomainObject : class
{
    private readonly ISecurityProvider<TDomainObject> securityProvider;

    private readonly IAsyncDal<TDomainObject, TIdent> dal;

    private readonly ISpecificationEvaluator specificationEvaluator;

    public GenericRepository(
            ISecurityProvider<TDomainObject> securityProvider,
            IAsyncDal<TDomainObject, TIdent> dal,
            ISpecificationEvaluator specificationEvaluator)
    {
        this.securityProvider = securityProvider;
        this.dal = dal;
        this.specificationEvaluator = specificationEvaluator;
    }

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.SaveAsync(domainObject, cancellationToken);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.InsertAsync(domainObject, id, cancellationToken);
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.securityProvider.CheckAccess(domainObject);

        await this.dal.RemoveAsync(domainObject, cancellationToken);
    }

    public IQueryable<TDomainObject> GetQueryable() => this.dal.GetQueryable().Pipe(this.securityProvider.InjectFilter);

    public TDomainObject Load(TIdent id) => this.dal.Load(id);

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        await this.dal.LoadAsync(id, cancellationToken);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default) =>
        await this.dal.RefreshAsync(domainObject, cancellationToken);

    public IQueryable<TProjection> GetQueryable<TProjection>(Specification<TDomainObject, TProjection> specification)
        => this.specificationEvaluator.GetQuery(this.GetQueryable(), specification);

    public async Task<TProjection?> SingleOrDefaultAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .SingleOrDefaultAsync(cancellationToken);

    public async Task<TProjection> SingleAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .SingleAsync(cancellationToken);

    public async Task<TProjection?> FirstOrDefaultAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .FirstOrDefaultAsync(cancellationToken);

    public async Task<TProjection> FirstAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .FirstAsync(cancellationToken);

    public async Task<int> CountAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification)
                      .CountAsync(cancellationToken);

    public INuFutureEnumerable<TProjection> GetFuture<TProjection>(
            Specification<TDomainObject, TProjection> specification) =>
            this.specificationEvaluator.GetFuture(this.GetQueryable(), specification);

    public INuFutureValue<TProjection> GetFutureValue<TProjection>(
            Specification<TDomainObject, TProjection> specification) =>
            this.specificationEvaluator.GetFutureValue(this.GetQueryable(), specification);

    public INuFutureValue<int> GetFutureCount<TProjection>(Specification<TDomainObject, TProjection> specification) =>
            this.specificationEvaluator.GetFutureValue(this.GetQueryable(), specification, x => x.Count());

    public async Task<List<TProjection>> GetListAsync<TProjection>(
            Specification<TDomainObject, TProjection> specification,
            CancellationToken cancellationToken) =>
            await this.specificationEvaluator.GetQuery(this.GetQueryable(), specification).ToListAsync(cancellationToken);
}
