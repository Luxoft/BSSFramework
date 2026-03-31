using CommonFramework.IdentitySource;

using Framework.Core;
using Framework.Database.EntityFramework.Sessions;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework;

public class EfAsyncDal<TDomainObject, TIdent>(
    IEfSession session,
    IIdentityInfo<TDomainObject, TIdent> identityInfo) : IAsyncDal<TDomainObject, TIdent>

    where TDomainObject : class
    where TIdent : notnull
{
    private DbContext NativeSession => session.NativeSession;

    public IQueryable<TDomainObject> GetQueryable()
    {
        return this.NativeSession.Set<TDomainObject>();
    }

    public TDomainObject Load(TIdent id) => this.LoadAsync(id).GetAwaiter().GetResult();

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken cancellationToken = default) =>
        (await this.NativeSession.FindAsync<TDomainObject>([id], cancellationToken))!; // Hack

    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken cancellationToken = default) =>
        await this.NativeSession.Entry(domainObject).ReloadAsync(cancellationToken);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        var state = session.NativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await session.NativeSession.AddAsync(domainObject, cancellationToken);
        }
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken cancellationToken = default)
    {
        if (identityInfo.Id.Getter(domainObject).IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        identityInfo.Id.Setter(domainObject, id);

        var state = this.NativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await this.NativeSession.AddAsync(domainObject, cancellationToken);
        }
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken cancellationToken = default)
    {
        this.CheckWrite();

        this.NativeSession.Remove(domainObject);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken cancellationToken)
    {
        this.CheckWrite();

        await this.NativeSession.Set<TDomainObject>()
                  .FromSqlRaw($"SELECT * FROM {nameof(TDomainObject)} WITH (UPDLOCK) WHERE Id = {0}", identityInfo.Id.Getter(domainObject))
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
