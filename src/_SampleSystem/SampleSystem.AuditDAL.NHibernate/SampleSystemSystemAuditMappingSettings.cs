using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditPersistentDomainObjectBase>(
        typeof(SampleSystemSystemAuditMappingSettings).Assembly,
        DatabaseName);
