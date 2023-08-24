using Framework.Persistent;

namespace Framework.DomainDriven;

public interface IDTOMappingVersionService<in TAuditPersistentDomainObjectBase, TVersion>
{
    TVersion GetVersion<TDomainObject>(TVersion mappingObjectVersion, TDomainObject domainObject)
            where TDomainObject : TAuditPersistentDomainObjectBase, IVersionObject<TVersion>;
}
