using Framework.Database;
using Framework.Database.NHibernate._MappingSettings;

using SampleSystem.AuditDomain.__Base;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditPersistentDomainObjectBase>(
        typeof(SampleSystemSystemAuditMappingSettings).Assembly,
        DatabaseName);
