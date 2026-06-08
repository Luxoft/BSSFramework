using Anch.Core;
using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.Providers;

using Framework.Database;

namespace Framework.Application.Repository.Generic;

public class GenericRepository<TDomainObject, TIdent>(
    IAsyncDal<TDomainObject, TIdent> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecurityProvider<TDomainObject> securityProvider)
    : IGenericRepository<TDomainObject, TIdent>
    where TDomainObject : class
{
    public async Task SaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        await this.CheckAccess(domainObject, ct);

        await dal.SaveAsync(domainObject, ct);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct)
    {
        await this.CheckAccess(domainObject, ct);

        await dal.InsertAsync(domainObject, id, ct);
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        await this.CheckAccess(domainObject, ct);

        await dal.RemoveAsync(domainObject, ct);
    }

    private ValueTask CheckAccess(TDomainObject domainObject, CancellationToken ct) =>
        securityProvider.CheckAccessAsync(domainObject, accessDeniedExceptionService, ct);

    public IQueryable<TDomainObject> GetQueryable() => dal.GetQueryable().Pipe(securityProvider.Inject);

    public TDomainObject Load(TIdent id) => dal.Load(id);

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) =>
        await dal.LoadAsync(id, ct);

    /// <summary>
    /// Re-read the state of the given instance from the underlying database.
    /// </summary>
    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) =>
        await dal.RefreshAsync(domainObject, ct);

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct) => await dal.LockAsync(domainObject, lockRole, ct);
}

