using System.Data;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

using Framework.Core;

using NHibernate.Cfg;

using Environment = NHibernate.Cfg.Environment;

namespace Framework.DomainDriven.NHibernate;

public class DefaultConfigurationInitializer(
    IDefaultConnectionStringSource defaultConnectionStringSource,
    IEnumerable<FluentMappingAssemblyInfo> assemblyInfoList,
    DefaultConfigurationInitializerSettings settings) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg)
    {
        Fluently
            .Configure(cfg)
            .Database(
                MsSqlConfiguration.MsSql2012
                                  .Dialect<EnhancedMsSql2012Dialect>()
                                  .Driver<Fix2100SqlClientDriver>()
                                  .Pipe(settings.IsolationLevel != null, f => f.IsolationLevel(settings.IsolationLevel!.Value))
                                  .IsolationLevel(IsolationLevel.ReadCommitted)
                                  .ConnectionString(defaultConnectionStringSource.ConnectionString))
            .Mappings(
                m =>
                {
                    assemblyInfoList.Select(assemblyInfo => assemblyInfo.Assembly)
                                    .Distinct()
                                    .Aggregate(m.FluentMappings, (fm, assembly) => fm.AddFromAssembly(assembly))
                                    .Conventions.AddFromAssemblyOf<EnumConvention>();
                })
            .ExposeConfiguration(
                c =>
                {
                    c.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(EnhancedLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
                    c.Properties.Add(Environment.SqlExceptionConverter, typeof(SQLExceptionConverter).AssemblyQualifiedName);
                    c.Properties.Add(Environment.CommandTimeout, settings.CommandTimeout.ToString());

                    if (settings.SqlTypesKeepDateTime != null)
                    {
                        c.Properties.Add(Environment.SqlTypesKeepDateTime, settings.SqlTypesKeepDateTime.ToString());
                    }
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
