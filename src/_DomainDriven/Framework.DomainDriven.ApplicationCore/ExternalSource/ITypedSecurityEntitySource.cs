namespace Framework.DomainDriven.ApplicationCore.ExternalSource;

public interface ITypedSecurityEntitySource
{
    IEnumerable<SecurityEntity> GetSecurityEntities();

    IEnumerable<SecurityEntity> GetSecurityEntitiesByIdents(IEnumerable<Guid> securityEntityIdents);

    IEnumerable<SecurityEntity> GetSecurityEntitiesWithMasterExpand(Guid startSecurityEntityId);

    bool IsExists (Guid securityEntityId);
}
