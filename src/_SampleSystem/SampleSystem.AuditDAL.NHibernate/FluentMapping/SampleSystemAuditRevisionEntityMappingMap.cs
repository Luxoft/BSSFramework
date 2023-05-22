using FluentNHibernate.Mapping;

using SampleSystem.AuditDomain;

namespace SampleSystem.AuditDAL.NHibernate.FluentMapping;

public class SampleSystemAuditRevisionEntityMappingMap : ClassMap<SampleSystemAuditRevisionEntity>
{
    public SampleSystemAuditRevisionEntityMappingMap()
    {
        this.Schema("appAudit");
        this.Id(x => x.Id).GeneratedBy.Increment().Access.CamelCaseField();
        this.Map(x => x.Author).Access.CamelCaseField();
        this.Map(x => x.RevisionDate).Access.CamelCaseField();
    }
}
