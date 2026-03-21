using Framework.BLL.Domain.IdentityObject;

namespace Framework.BLL.DTO.MappingService;

public interface IDTOMappingVersionService<in TAuditPersistentDomainObjectBase, TVersion>
{
    TVersion GetVersion<TDomainObject>(TVersion mappingObjectVersion, TDomainObject domainObject)
            where TDomainObject : TAuditPersistentDomainObjectBase, IVersionObject<TVersion>;
}
