using Framework.QueryableSource;

namespace Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

public abstract class TypedSecurityContextStorageBase<TSecurityContext>(
    IQueryableSource queryableSource,
    LocalStorage<TSecurityContext> localStorage)
    : ITypedSecurityContextStorage
    where TSecurityContext : class, ISecurityContext
{
    protected abstract SecurityContextData CreateSecurityContextData(TSecurityContext securityContext);

    public IEnumerable<SecurityContextData> GetSecurityContexts()
    {
        return queryableSource.GetQueryable<TSecurityContext>().ToList().Select(this.CreateSecurityContextData);
    }

    public IEnumerable<SecurityContextData> GetSecurityContextsByIdents(IEnumerable<Guid> preSecurityEntityIdents)
    {
        var securityEntityIdents = preSecurityEntityIdents.ToList();

        return queryableSource.GetQueryable<TSecurityContext>().Where(obj => securityEntityIdents.Contains(obj.Id)).ToList()
                              .Select(this.CreateSecurityContextData);
    }

    public IEnumerable<SecurityContextData> GetSecurityContextsWithMasterExpand(Guid startSecurityEntityId)
    {
        var securityObject = queryableSource.GetQueryable<TSecurityContext>().Single(obj => obj.Id == startSecurityEntityId);

        return this.GetSecurityContextsWithMasterExpand(securityObject).Select(this.CreateSecurityContextData);
    }

    public bool IsExists(Guid securityContextId)
    {
        return localStorage.IsExists(securityContextId)
               || queryableSource.GetQueryable<TSecurityContext>().Any(securityContext => securityContext.Id == securityContextId);
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityContextsWithMasterExpand(TSecurityContext startSecurityObject);
}
