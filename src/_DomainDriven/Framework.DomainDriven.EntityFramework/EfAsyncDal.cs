using Framework.Core;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.Lock;
using Framework.GenericQueryable;
using Framework.Persistent;

using GenericQueryable;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public class EfAsyncDal<TDomainObject, TIdent>(
    IEfSession session,
    IExpressionVisitorContainer expressionVisitorContainer,
    IGenericQueryableExecutor genericQueryableExecutor)
    : IAsyncDal<TDomainObject, TIdent>
    where TDomainObject : class, IIdentityObject<TIdent>
{
    private DbContext NativeSession => session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        var queryable = (IQueryable<TDomainObject>)this.NativeSession.Set<TDomainObject>();

        var queryProvider = (queryable.Provider as VisitedEfQueryProvider)
                            .FromMaybe("Register VisitedQueryProvider in Nhib configuration");

        queryProvider.Visitor = expressionVisitorContainer.Visitor;
        queryProvider.GenericQueryableExecutor = genericQueryableExecutor;

        return queryable;
    }

    public virtual TDomainObject Load(TIdent id) => this.LoadAsync(id).GetAwaiter().GetResult();

    public virtual async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        await this.NativeSession.FindAsync<TDomainObject>([id], cancellationToken); // Hack

    public virtual async Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default) =>
        await this.NativeSession.Entry(domainObject).ReloadAsync(cancellationToken);

    public virtual async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        this.NativeSession.Attach(domainObject);

        await this.NativeSession.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        this.NativeSession.Attach(domainObject);

        await this.NativeSession.SaveChangesAsync(cancellationToken); // Hack
    }

    public virtual async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        this.NativeSession.Remove(domainObject);
        await this.NativeSession.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
    {
        this.CheckWrite();

        await this.NativeSession.Set<TDomainObject>()
                  .FromSqlRaw($"SELECT * FROM {nameof(TDomainObject)} WITH (UPDLOCK) WHERE Id = {0}", domainObject.Id)
                  .ToListAsync(cancellationToken);
        //throw new NotImplementedException();

        //await this.NativeSession.LockAsync(domainObject, lockRole.ToLockMode(), cancellationToken);
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
