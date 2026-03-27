

using System.Data;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Framework.DomainDriven.NHibernate;

public record DefaultConfigurationInitializerSettings
{
    public int CommandTimeout { get; init; } = 1200;

    public bool? SqlTypesKeepDateTime { get; init; } = null;

    public IsolationLevel? IsolationLevel { get; init; } = null;

    public int? BatchSize { get; init; } = null;

    public bool ComponentConventionEnable { get; init; } = true;

    public IReadOnlyList<Assembly> FluentAssemblyList { get; init; } = [];

    public Action<MappingConfiguration> RawMappingAction { get; init; } = _ => { };

    public Action<MsSqlConfiguration> RawDatabaseAction { get; init; } = _ => { };
}
