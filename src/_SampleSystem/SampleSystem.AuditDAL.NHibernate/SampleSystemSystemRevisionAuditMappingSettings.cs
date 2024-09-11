using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public class SampleSystemSystemRevisionAuditMappingSettings(string databaseName)
        : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>(
            typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly,
            new DatabaseName(databaseName, "appAudit"));
}
