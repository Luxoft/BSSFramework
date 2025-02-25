using Framework.Core;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.Lock;
using Framework.GenericQueryable;
using Framework.Persistent;

using NHibernate;

namespace Framework.DomainDriven.NHibernate;

public class NHibAsyncDal<TDomainObject, TIdent>(
    INHibSession session,
    IExpressionVisitorContainer expressionVisitorContainer,
    IGenericQueryableExecutor genericQueryableExecutor)
    : IAsyncDal<TDomainObject, TIdent>
    where TDomainObject : class, IIdentityObject<TIdent>
{
    private ISession NativeSession => session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        var queryable = this.NativeSession.Query<TDomainObject>();

        var queryProvider = (queryable.Provider as VisitedQueryProvider)
                            .FromMaybe("Register VisitedQueryProvider in Nhib configuration");

        queryProvider.Visitor = expressionVisitorContainer.Visitor;
        queryProvider.GenericQueryableExecutor = genericQueryableExecutor;

        return queryable;
    }

    public virtual TDomainObject Load(TIdent id) => this.NativeSession.Load<TDomainObject>(id);

    public virtual async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        await this.NativeSession.LoadAsync<TDomainObject>(id, cancellationToken);

    public virtual async Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default) =>
        await this.NativeSession.RefreshAsync(domainObject, cancellationToken);

    public virtual async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        await this.NativeSession.SaveOrUpdateAsync(domainObject, cancellationToken);

        session.RegisterModified(domainObject, ModificationType.Save);
    }

    public virtual async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        await this.NativeSession.SaveAsync(domainObject, id, cancellationToken);

        session.RegisterModified(domainObject, ModificationType.Save);
    }

    public virtual async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        session.RegisterModified(domainObject, ModificationType.Remove);

        await this.NativeSession.DeleteAsync(domainObject, cancellationToken);
    }

    public virtual async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
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
