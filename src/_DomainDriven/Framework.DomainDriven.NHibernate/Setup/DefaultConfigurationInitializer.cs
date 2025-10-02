using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Framework.Core;

using NHibernate.Cfg;

using Environment = NHibernate.Cfg.Environment;

namespace Framework.DomainDriven.NHibernate;

public class DefaultConfigurationInitializer(
    IDefaultConnectionStringSource defaultConnectionStringSource,
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
                                  .PipeMaybe(settings.IsolationLevel, (f, v) => f.IsolationLevel(v))
                                  .PipeMaybe(settings.BatchSize, (f, v) => f.AdoNetBatchSize(v))
                                  .ConnectionString(defaultConnectionStringSource.ConnectionString)
                                  .Self(settings.RawDatabaseAction))
            .Mappings(
                m =>
                {
                    var conventions = settings.FluentAssemblyList.Distinct()
                                              .Aggregate(m.FluentMappings, (fm, assembly) => fm.AddFromAssembly(assembly))
                                              .Conventions;

                    conventions.Add(new EnumConvention());

                    if (settings.ComponentConventionEnable)
                    {
                        conventions.Add(new ComponentConvention());
                    }

                    settings.RawMappingAction(m);
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
}
