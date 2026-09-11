using Anch.IdentitySource;

using Framework.Core;

using Microsoft.EntityFrameworkCore;

namespace Framework.Database.EntityFramework;

public class EfAsyncDal<TDomainObject, TIdent>(
    IDBSession session,
    DbContext nativeSession,
    IIdentityInfo<TDomainObject, TIdent> identityInfo) : IAsyncDal<TDomainObject, TIdent>

    where TDomainObject : class
    where TIdent : notnull
{
    public IQueryable<TDomainObject> GetQueryable() => nativeSession.Set<TDomainObject>();

    public TDomainObject Load(TIdent id) => nativeSession.Find<TDomainObject>(id) ?? throw new InvalidOperationException($"Entity of type {typeof(TDomainObject).Name} with ID {id} not found.");

    public async Task<TDomainObject> LoadAsync(TIdent id, CancellationToken ct) =>
        (await nativeSession.FindAsync<TDomainObject>([id], ct) ?? throw new InvalidOperationException($"Entity of type {typeof(TDomainObject).Name} with ID {id} not found.")); // Hack

    public async Task RefreshAsync(TDomainObject domainObject, CancellationToken ct) =>
        await nativeSession.Entry(domainObject).ReloadAsync(ct);

    public async Task SaveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        var state = nativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await nativeSession.AddAsync(domainObject, ct);
        }
    }

    public async Task InsertAsync(TDomainObject domainObject, TIdent id, CancellationToken ct)
    {
        if (id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "The given identifier is not initialized");
        }

        this.CheckWrite();

        identityInfo.Id.Setter(domainObject, id);

        var state = nativeSession.Entry(domainObject).State;

        if (state == EntityState.Detached)
        {
            await nativeSession.AddAsync(domainObject, ct);
        }
    }

    public async Task RemoveAsync(TDomainObject domainObject, CancellationToken ct)
    {
        this.CheckWrite();

        nativeSession.Remove(domainObject);
    }

    public async Task LockAsync(TDomainObject domainObject, LockRole lockRole, CancellationToken ct)
    {
        this.CheckWrite();

        var entityType = nativeSession.Model.FindEntityType(typeof(TDomainObject))
                          ?? throw new InvalidOperationException($"Entity type \"{typeof(TDomainObject)}\" not found.");

        var tableName = entityType.GetTableName()
                         ?? throw new InvalidOperationException($"Table name for entity type \"{typeof(TDomainObject)}\" not found.");

        var schema = entityType.GetSchema();

        var fullTableName = schema is null ? $"[{tableName}]" : $"[{schema}].[{tableName}]";

        await nativeSession.Set<TDomainObject>()
                  .FromSqlRaw($"SELECT * FROM {fullTableName} WITH (UPDLOCK) WHERE Id = {{0}}", identityInfo.Id.Getter(domainObject))
                  .ToListAsync(ct);
    }

    private void CheckWrite()
    {
        if (session.SessionMode != DBSessionMode.Write)
        {
            throw new InvalidOperationException("Invalid session mode. Expected Write.");
        }
    }
}
