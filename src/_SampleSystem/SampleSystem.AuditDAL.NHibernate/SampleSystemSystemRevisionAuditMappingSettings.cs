using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public record SampleSystemSystemRevisionAuditMappingSettings(string DatabaseName)
        : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>(
            typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly,
            new DatabaseName(DatabaseName, "appAudit"));
}
