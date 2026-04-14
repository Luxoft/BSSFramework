using Framework.Database;
using Framework.Database.NHibernate.Mapping;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditPersistentDomainObjectBase>(
        typeof(SampleSystemSystemAuditMappingSettings).Assembly,
        DatabaseName);
