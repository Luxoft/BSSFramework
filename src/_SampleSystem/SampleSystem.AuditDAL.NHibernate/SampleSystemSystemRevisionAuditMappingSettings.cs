using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate
{
    public class SampleSystemSystemRevisionAuditMappingSettings : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>
    {
        public SampleSystemSystemRevisionAuditMappingSettings(string databaseName)
                : base(typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly, new DatabaseName(databaseName, "appAudit"))
        {
        }
    }
}
