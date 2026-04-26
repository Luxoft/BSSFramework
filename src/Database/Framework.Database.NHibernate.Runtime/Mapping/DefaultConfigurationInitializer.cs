using Anch.Core;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Framework.Core;
using Framework.Database.NHibernate.Dialect;
using Framework.Database.NHibernate.Fix2100;

using NHibernate.Cfg;

using Environment = NHibernate.Cfg.Environment;

namespace Framework.Database.NHibernate.Mapping;

public class DefaultConfigurationInitializer(
    IDefaultConnectionStringSource defaultConnectionStringSource,
    DatabaseSettings databaseSettings,
    NHibernateSettings nhibernateSettings) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg) =>
        Fluently
            .Configure(cfg)
            .Database(
                MsSqlConfiguration.MsSql2012
                                  .Dialect<EnhancedMsSql2012Dialect>()
                                  .Driver<Fix2100SqlClientDriver>()
                                  .PipeMaybe(databaseSettings.IsolationLevel, (f, v) => f.IsolationLevel(v))
                                  .PipeMaybe(databaseSettings.BatchSize, (f, v) => f.AdoNetBatchSize(v))
                                  .ConnectionString(defaultConnectionStringSource.ConnectionString)
                                  .Self(nhibernateSettings.RawDatabaseAction))
            .Mappings(
                m =>
                {
                    var conventions = nhibernateSettings.FluentAssemblyList.Distinct()
                                              .Aggregate(m.FluentMappings, (fm, assembly) => fm.AddFromAssembly(assembly))
                                              .Conventions;

                    conventions.Add(new EnumConvention());

                    if (nhibernateSettings.ComponentConventionEnable)
                    {
                        conventions.Add(new ComponentConvention());
                    }

                    nhibernateSettings.RawMappingAction(m);
                })
            .ExposeConfiguration(
                c =>
                {
                    c.Properties.Add(Environment.LinqToHqlGeneratorsRegistry, typeof(EnhancedLinqToHqlGeneratorsRegistry).AssemblyQualifiedName);
                    c.Properties.Add(Environment.SqlExceptionConverter, typeof(SqlExceptionConverter).AssemblyQualifiedName);
                    c.Properties.Add(Environment.CommandTimeout, databaseSettings.CommandTimeout.ToString());

                    if (nhibernateSettings.SqlTypesKeepDateTime != null)
                    {
                        c.Properties.Add(Environment.SqlTypesKeepDateTime, nhibernateSettings.SqlTypesKeepDateTime.ToString());
                    }
                })
            .BuildConfiguration();
}
