using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace Framework.Database.NHibernate.Mapping;

public record NHibernateSettings
{
    public bool? SqlTypesKeepDateTime { get; init; } = null;

    public bool ComponentConventionEnable { get; init; } = true;

    public IReadOnlyList<Assembly> FluentAssemblyList { get; init; } = [];

    public Action<MappingConfiguration> RawMappingAction { get; init; } = _ => { };

    public Action<MsSqlConfiguration> RawDatabaseAction { get; init; } = _ => { };
}
