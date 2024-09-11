using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public class SampleSystemSystemAuditMappingSettings(string databaseName) : MappingSettings<SystemAuditPersistentDomainObjectBase>(
        typeof(SampleSystemSystemAuditMappingSettings).Assembly,
        new DatabaseName(databaseName, "appAudit"));
}
