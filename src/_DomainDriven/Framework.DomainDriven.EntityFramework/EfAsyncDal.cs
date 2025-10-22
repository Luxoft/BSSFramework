using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Persistent;

using Microsoft.EntityFrameworkCore;

namespace Framework.DomainDriven.EntityFramework;

public class EfAsyncDal<TDomainObject, TIdent>(IEfSession session) : IAsyncDal<TDomainObject, TIdent>
    where TDomainObject : class, IIdentityObject<TIdent>
{
    private DbContext NativeSession => session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        return this.NativeSession.Set<TDomainObject>();
    }

    public virtual TDomainObject Load(TIdent id) => this.LoadAsync(id).GetAwaiter().GetResult();

    public virtual async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        (await this.NativeSession.FindAsync<TDomainObject>([id], cancellationToken))!; // Hack

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
        if (domainObject.Id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        var state = this.NativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await this.NativeSession.AddAsync(domainObject, cancellationToken);
        }
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
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
