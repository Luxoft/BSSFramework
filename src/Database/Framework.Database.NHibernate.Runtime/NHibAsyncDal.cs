using Anch.Core;
using Anch.GenericQueryable.NHibernate;
using Anch.GenericQueryable.Services;
using Anch.IdentitySource;

using Framework.Core;

using NHibernate;

namespace Framework.Database.NHibernate;

public class NHibAsyncDal<TDomainObject, TIdent>(
    IDBSession session,
    ISession nativeSession,
    IExpressionVisitorContainer expressionVisitorContainer,
    IGenericQueryableExecutor genericQueryableExecutor,
    IIdentityInfo<TDomainObject, TIdent> identityInfo)
    : IAsyncDal<TDomainObject, TIdent>
    where TDomainObject : class
    where TIdent : notnull
{
    public IQueryable<TDomainObject> GetQueryable()
    {
        var queryable = nativeSession.Query<TDomainObject>();

        var queryProvider = (queryable.Provider as VisitedNHibQueryProvider).FromMaybe(() => "Register VisitedQueryProvider in Nhib configuration");

        queryProvider.Visitor = expressionVisitorContainer.Visitor;
        queryProvider.Executor = genericQueryableExecutor;

        return queryable;
    }

    public TDomainObject Load(TIdent id) => nativeSession.Load<TDomainObject>(id);

    public Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) =>
        nativeSession.LoadAsync<TDomainObject>(id, ct);

    public Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) =>
        nativeSession.RefreshAsync(domainObject, ct);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        await this.ActualSaveAsync(domainObject, ct);
    }

    private Task ActualSaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        if (!nativeSession.Contains(domainObject))
        {
            var id = identityInfo.Id.Getter(domainObject);

            if (!EqualityComparer<TIdent>.Default.Equals(id, default))
            {
                return nativeSession.SaveAsync(domainObject, id, ct);
            }
        }

        return nativeSession.SaveOrUpdateAsync(domainObject, ct);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        await this.ActualInsertAsync(domainObject, id, ct);
    }

    public Task ActualInsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct)
    {
        this.CheckWrite();

        if (EqualityComparer<TIdent>.Default.Equals(id, default))
        {
            return nativeSession.SaveOrUpdateAsync(domainObject, ct);
        }
        else
        {
            return nativeSession.SaveAsync(domainObject, id, ct);
        }
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        await nativeSession.DeleteAsync(domainObject, ct);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct)
    {
        this.CheckWrite();

        await nativeSession.LockAsync(domainObject, lockRole.ToLockMode(), ct);
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
