using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public abstract class TypedSecurityEntitySourceBase<TSecurityContext>(
    [DisabledSecurity] IRepository<TSecurityContext> securityContextRepository,
    LocalStorage<TSecurityContext> localStorage)
    : ITypedSecurityEntitySource
    where TSecurityContext : class, ISecurityContext
{
    protected abstract SecurityEntity CreateSecurityEntity(TSecurityContext securityContext);

    public IEnumerable<SecurityEntity> GetSecurityEntities()
    {
        return securityContextRepository.GetQueryable().ToList().Select(this.CreateSecurityEntity);
    }

    public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> preSecurityEntityIdents)
    {
        var securityEntityIdents = preSecurityEntityIdents.ToList();

        return securityContextRepository.GetQueryable().Where(obj => securityEntityIdents.Contains(obj.Id)).ToList()
                   .Select(this.CreateSecurityEntity);
    }

    public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
    {
        var securityObject = securityContextRepository.GetQueryable().Single(obj => obj.Id == startSecurityEntityId);

        return this.GetSecurityEntitiesWithMasterExpand(securityObject).Select(this.CreateSecurityEntity);
    }

    public bool IsExists(Guid securityEntityId)
    {
        return localStorage.IsExists(securityEntityId)
               || securityContextRepository.GetQueryable().Any(sc => sc.Id == securityEntityId);
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject);
}
