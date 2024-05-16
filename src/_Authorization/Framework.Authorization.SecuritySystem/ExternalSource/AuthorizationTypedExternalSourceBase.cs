using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public abstract class AuthorizationTypedExternalSourceBase<TSecurityContext> : IAuthorizationTypedExternalSource
    where TSecurityContext : class, IIdentityObject<Guid>, ISecurityContext
{
    private readonly IRepository<TSecurityContext> securityContextRepository;

    protected AuthorizationTypedExternalSourceBase(
        [FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<TSecurityContext> securityContextRepository)
    {
        this.securityContextRepository = securityContextRepository;
    }

    protected abstract SecurityEntity CreateSecurityEntity(TSecurityContext securityContext);

    public IEnumerable<SecurityEntity> GetSecurityEntities()
    {
        return this.securityContextRepository.GetQueryable().ToList().Select(this.CreateSecurityEntity);
    }

    public IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> preSecurityEntityIdents)
    {
        var securityEntityIdents = preSecurityEntityIdents.ToList();

        return this.securityContextRepository.GetQueryable().Where(obj => securityEntityIdents.Contains(obj.Id)).ToList()
                   .Select(this.CreateSecurityEntity);
    }

    public IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId)
    {
        var securityObject = this.securityContextRepository.GetQueryable().Single(obj => obj.Id == startSecurityEntityId);

        return this.GetSecurityEntitiesWithMasterExpand(securityObject).Select(this.CreateSecurityEntity);
    }

    public bool IsExists(Guid securityEntityId)
    {
        return this.securityContextRepository.GetQueryable().Any(sc => sc.Id == securityEntityId);
    }

    protected abstract IEnumerable<TSecurityContext> GetSecurityEntitiesWithMasterExpand(TSecurityContext startSecurityObject);
}
