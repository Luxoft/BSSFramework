using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

using Framework.DomainDriven.NHibernate;

using Microsoft.Extensions.Configuration;

using NHibernate.Cfg;

using Environment = NHibernate.Cfg.Environment;

namespace SampleSystem.Generated.DAL.NHibernate;

public interface IDefaultConnectionStringSource
{
    string ConnectionString { get; }
}

public record DefaultConnectionStringSettings(string Name);

public class DefaultConnectionStringSource(IConfiguration configuration, DefaultConnectionStringSettings settings)
    : ConnectionStringSource(configuration, settings.Name);

public class ConnectionStringSource(IConfiguration configuration, string name)
{
    public string ConnectionString => configuration.GetConnectionString(name);
}

public class SampleSystemConfigurationInitializer(string connectionString) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg)
    {
        Fluently
            .Configure(cfg)
            .Database(
                MsSqlConfiguration.MsSql2012
                                  .Dialect<EnhancedMsSql2012Dialect>()
                                  .Driver<Fix2100SqlClientDriver>()
                                  .ConnectionString(connectionString))
            .Mappings(
                m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<SampleSystemConfigurationInitializer>()
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
