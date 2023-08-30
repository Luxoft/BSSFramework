using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public class SampleSystemSystemAuditMappingSettings : MappingSettings<SystemAuditPersistentDomainObjectBase>
    {
        public SampleSystemSystemAuditMappingSettings(string databaseName)
                : base(typeof(SampleSystemSystemAuditMappingSettings).Assembly, new DatabaseName(databaseName, "appAudit"))
        {
        }
    }
}
