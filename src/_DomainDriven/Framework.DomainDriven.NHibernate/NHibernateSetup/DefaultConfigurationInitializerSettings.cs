using System.Data;

namespace Framework.DomainDriven.NHibernate;

public record DefaultConfigurationInitializerSettings
{
    public int CommandTimeout { get; init; } = 1200;

    public bool? SqlTypesKeepDateTime { get; init; } = null;

    public IsolationLevel? IsolationLevel { get; init; } = null;
}
