using System.Data;

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
                                  .Pipe(settings.IsolationLevel != null, f => f.IsolationLevel(settings.IsolationLevel!.Value))
                                  .IsolationLevel(IsolationLevel.ReadCommitted)
                                  .ConnectionString(defaultConnectionStringSource.ConnectionString))
            .Mappings(
                m =>
                {
                    settings.FluentInitAction(m.FluentMappings);

                    settings.FluentAssemblyList.Distinct()
                            .Aggregate(m.FluentMappings, (fm, assembly) => fm.AddFromAssembly(assembly))
                            .Conventions.Add(new EnumConvention()//, new ComponentConvention()
                                );
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
