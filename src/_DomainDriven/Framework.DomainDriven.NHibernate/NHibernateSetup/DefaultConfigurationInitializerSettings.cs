#nullable enable

using System.Data;
using System.Reflection;

using FluentNHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public record DefaultConfigurationInitializerSettings
{
    public int CommandTimeout { get; init; } = 1200;

    public bool? SqlTypesKeepDateTime { get; init; } = null;

    public IsolationLevel? IsolationLevel { get; init; } = null;

    public IReadOnlyList<Assembly> FluentAssemblyList { get; init; } = [];

    public Action<FluentConfiguration> FluentInitAction { get; init; } = _ => { };
}
