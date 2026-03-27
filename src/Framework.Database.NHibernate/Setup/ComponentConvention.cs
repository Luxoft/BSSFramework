using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace Framework.DomainDriven.NHibernate;

public sealed class ComponentConvention : IComponentConvention, IComponentConventionAcceptance
{
    public void Apply(IComponentInstance instance)
    {
        foreach (var property in instance.Properties)
        {
            property.Column($"{instance.Property.Name}{property.Name}");
        }
    }

    public void Accept(IAcceptanceCriteria<IComponentInspector> criteria)
    {
        criteria.Expect(_ => true);
    }
}
