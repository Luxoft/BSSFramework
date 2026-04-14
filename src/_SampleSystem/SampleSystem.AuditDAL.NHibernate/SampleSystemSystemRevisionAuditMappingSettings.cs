using Framework.Database;
using Framework.Database.NHibernate.Mapping;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate;

public record SampleSystemSystemRevisionAuditMappingSettings(DatabaseName DatabaseName)
    : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>(
        typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly,
        DatabaseName);
