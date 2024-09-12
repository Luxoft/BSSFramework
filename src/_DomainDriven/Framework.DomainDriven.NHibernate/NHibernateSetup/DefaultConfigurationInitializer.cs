using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

using NHibernate.Cfg;

using Environment = NHibernate.Cfg.Environment;

namespace Framework.DomainDriven.NHibernate;

public class DefaultConfigurationInitializer(
    IDefaultConnectionStringSource defaultConnectionStringSource,
    IEnumerable<FluentMappingAssemblyInfo> assemblyInfoList) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg)
    {
        Fluently
            .Configure(cfg)
            .Database(
                MsSqlConfiguration.MsSql2012
                                  .Dialect<EnhancedMsSql2012Dialect>()
                                  .Driver<Fix2100SqlClientDriver>()
                                  .ConnectionString(defaultConnectionStringSource.ConnectionString))
            .Mappings(
                m =>
                {
                    assemblyInfoList.Aggregate(m.FluentMappings, (fm, ai) => fm.AddFromAssembly(ai.Assembly))
                                    .Conventions.AddFromAssemblyOf<EnumConvention>();
                })
            .ExposeConfiguration(
                c =>
                {
                    c.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(EnhancedLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
                    c.Properties.Add(Environment.SqlExceptionConverter, typeof(SQLExceptionConverter).AssemblyQualifiedName);
                    c.Properties.Add(Environment.CommandTimeout, 1200.ToString());
                    c.Properties.Add(Environment.SqlTypesKeepDateTime, bool.TrueString);
                })
            .BuildConfiguration();
    }

    private class EnumConvention : IUserTypeConvention
    {
        public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria) =>
            criteria.Expect(
                x => x.Property.PropertyType.IsEnum
                     || (x.Property.PropertyType.IsGenericType
                         && x.Property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                         && x.Property.PropertyType.GetGenericArguments()[0].IsEnum));

        public void Apply(IPropertyInstance instance) => instance.CustomType(instance.Property.PropertyType);
    }
}
