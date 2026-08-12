using Framework.Application.Domain;

namespace Framework.BLL.DTOMapping.Services;

public interface IDTOMappingVersionService<in TAuditPersistentDomainObjectBase, TVersion>
{
    TVersion GetVersion<TDomainObject>(TVersion mappingObjectVersion, TDomainObject domainObject)
        where TDomainObject : TAuditPersistentDomainObjectBase, IVersionObject<TVersion>;
}
