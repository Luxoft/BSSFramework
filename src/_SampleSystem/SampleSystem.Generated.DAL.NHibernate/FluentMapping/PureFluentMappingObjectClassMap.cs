using FluentNHibernate.Mapping;

using SampleSystem.Domain;

namespace SampleSystem.Generated.DAL.NHibernate.FluentMapping;

public sealed class PureFluentMappingObjectClassMap : ClassMap<PureFluentMappingObject>
{
    public PureFluentMappingObjectClassMap()
    {
        this.Schema("app");

        this.Id(x => x.Id).GeneratedBy.GuidComb();

        this.Component(
                x => x.Period,
                m =>
                {
                    m.Map(x => x.StartDate).Column("startDate").Access.CamelCaseField().CustomType("timestamp");
                    m.Map(x => x.EndDate).Column("endDate").Access.CamelCaseField().CustomType("timestamp");
                })
            .Access.CamelCaseField();

        this.Component(x => x.Period123, m => { })
            .Access.CamelCaseField();

        this.Component(x => x.Period456, m => { })
            .Access.CamelCaseField();
    }
}
