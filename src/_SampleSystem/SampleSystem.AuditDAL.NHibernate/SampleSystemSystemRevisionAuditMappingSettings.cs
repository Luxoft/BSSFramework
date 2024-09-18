using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemRevisionAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>(
        typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly,
        DatabaseName);
