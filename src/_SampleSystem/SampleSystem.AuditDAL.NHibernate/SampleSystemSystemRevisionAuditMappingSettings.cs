using Framework.Database;
using Framework.Database.NHibernate._MappingSettings;

using SampleSystem.AuditDomain.__Base;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemRevisionAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>(
        typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly,
        DatabaseName);
