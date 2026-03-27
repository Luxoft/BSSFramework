using CommonFramework;
using CommonFramework.IdentitySource;

using Framework.Core;
using Framework.Database.ExpressionVisitorContainer;
using Framework.Database.NHibernate.Sessions;

using GenericQueryable.NHibernate;
using GenericQueryable.Services;

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

        var queryProvider = (queryable.Provider as VisitedNHibQueryProvider)
                            .FromMaybe(() => "Register VisitedQueryProvider in Nhib configuration");

        queryProvider.Visitor = expressionVisitorContainer.Visitor;
        queryProvider.Executor = genericQueryableExecutor;

        return queryable;
    }

    public TDomainObject Load(TIdent id) => this.NativeSession.Load<TDomainObject>(id);

    public Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        this.NativeSession.LoadAsync<TDomainObject>(id, cancellationToken);

    public Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default) =>
        this.NativeSession.RefreshAsync(domainObject, cancellationToken);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken)
    {
        this.CheckWrite();

        if (!session.NativeSession.Contains(domainObject))
        {
            var id = identityInfo.Id.Getter(domainObject);

            if (!EqualityComparer<TIdent>.Default.Equals(id, default))
            {
                await session.NativeSession.SaveAsync(domainObject, id, cancellationToken);
            }
        }

        await session.NativeSession.SaveOrUpdateAsync(domainObject, cancellationToken);
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        await this.NativeSession.SaveAsync(domainObject, id, cancellationToken);
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        await this.NativeSession.DeleteAsync(domainObject, cancellationToken);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
    {
        this.CheckWrite();

        await this.NativeSession.LockAsync(domainObject, lockRole.ToLockMode(), cancellationToken);
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
