using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public interface IAuthorizationTypedExternalSource
{
    IEnumerable<SecurityEntity> GetSecurityEntities();

    IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> securityEntityIdents);

    IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId);
}
