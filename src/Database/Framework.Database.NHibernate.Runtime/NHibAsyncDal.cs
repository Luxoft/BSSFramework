using Anch.Core;
using Anch.GenericQueryable.NHibernate;
using Anch.GenericQueryable.Services;
using Anch.IdentitySource;

using Framework.Core;
using Framework.Database.NHibernate.Sessions;

using NHibernate;

namespace Framework.Database.NHibernate;

public class NHibAsyncDal<TDomainObject, TIdent>(
    INHibSession session,
    IExpressionVisitorContainer expressionVisitorContainer,
    IGenericQueryableExecutor genericQueryableExecutor,
    IIdentityInfo<TDomainObject, TIdent> identityInfo)
    : IAsyncDal<TDomainObject, TIdent>
    where TDomainObject : class
    where TIdent : notnull
{
    private ISession NativeSession => session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        var queryable = this.NativeSession.Query<TDomainObject>();

        var queryProvider = (queryable.Provider as VisitedNHibQueryProvider).FromMaybe(() => "Register VisitedQueryProvider in Nhib configuration");

        queryProvider.Visitor = expressionVisitorContainer.Visitor;
        queryProvider.Executor = genericQueryableExecutor;

        return queryable;
    }

    public TDomainObject Load(TIdent id) => this.NativeSession.Load<TDomainObject>(id);

    public Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) =>
        this.NativeSession.LoadAsync<TDomainObject>(id, ct);

    public Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) =>
        this.NativeSession.RefreshAsync(domainObject, ct);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        await this.ActualSaveAsync(domainObject, ct);
    }

    private Task ActualSaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        if (!session.NativeSession.Contains(domainObject))
        {
            var id = identityInfo.Id.Getter(domainObject);

            if (!EqualityComparer<TIdent>.Default.Equals(id, default))
            {
                return session.NativeSession.SaveAsync(domainObject, id, ct);
            }
        }

        return session.NativeSession.SaveOrUpdateAsync(domainObject, ct);
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
            return session.NativeSession.SaveOrUpdateAsync(domainObject, ct);
        }
        else
        {
            return this.NativeSession.SaveAsync(domainObject, id, ct);
        }
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        await this.NativeSession.DeleteAsync(domainObject, ct);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct)
    {
        this.CheckWrite();

        await this.NativeSession.LockAsync(domainObject, lockRole.ToLockMode(), ct);
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}

