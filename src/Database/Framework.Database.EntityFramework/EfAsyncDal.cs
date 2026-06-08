using Anch.IdentitySource;

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

    public IQueryable<TDomainObject> GetQueryable() => this.NativeSession.Set<TDomainObject>();

    public TDomainObject Load(TIdent id) => this.NativeSession.Find<TDomainObject>(id) ?? throw new InvalidOperationException($"Entity of type {typeof(TDomainObject).Name} with ID {id} not found.");

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) =>
        (await this.NativeSession.FindAsync<TDomainObject>([id], ct) ?? throw new InvalidOperationException($"Entity of type {typeof(TDomainObject).Name} with ID {id} not found.")); // Hack

    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) =>
        await this.NativeSession.Entry(domainObject).ReloadAsync(ct);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        var state = session.NativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await session.NativeSession.AddAsync(domainObject, ct);
        }
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct)
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
            await this.NativeSession.AddAsync(domainObject, ct);
        }
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        this.NativeSession.Remove(domainObject);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct)
    {
        this.CheckWrite();

        await this.NativeSession.Set<TDomainObject>()
                  .FromSqlRaw($"SELECT * FROM {nameof(TDomainObject)} WITH (UPDLOCK) WHERE Id = {0}", identityInfo.Id.Getter(domainObject))
                  .ToListAsync(ct);

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

