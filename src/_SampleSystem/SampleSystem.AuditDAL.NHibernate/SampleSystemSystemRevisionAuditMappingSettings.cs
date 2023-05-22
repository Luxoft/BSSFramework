using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;

using NHibernate.Cfg;

using SampleSystem.AuditDAL.NHibernate.FluentMapping;
using SampleSystem.AuditDomain;

using Environment = System.Environment;

namespace SampleSystem.AuditDAL.NHibernate
{
    public class SampleSystemSystemRevisionAuditMappingSettings : MappingSettings<SystemAuditRevisionPersistentDomainObjectBase>
    {
        public SampleSystemSystemRevisionAuditMappingSettings(string databaseName)
                : base(typeof(SystemAuditRevisionPersistentDomainObjectBase).Assembly, new DatabaseName(databaseName, "appAudit"))
        {
        }

        public override void InitMapping(Configuration cfg)
        {
            base.InitMapping(cfg);

            Fluently
                .Configure(cfg)
                .Mappings(
                    m =>
                    {
                        m.FluentMappings.Add<SampleSystemAuditRevisionEntityMappingMap>();
                    })
                .BuildConfiguration();
        }
    }
}
