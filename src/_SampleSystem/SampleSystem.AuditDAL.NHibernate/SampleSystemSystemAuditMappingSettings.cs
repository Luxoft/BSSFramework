using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public record SampleSystemSystemAuditMappingSettings(string DatabaseName) : MappingSettings<SystemAuditPersistentDomainObjectBase>(
        typeof(SampleSystemSystemAuditMappingSettings).Assembly,
        new DatabaseName(DatabaseName, "appAudit"));
}
