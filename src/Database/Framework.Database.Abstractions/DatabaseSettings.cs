using System.Data;

namespace Framework.Database;

public record DatabaseSettings
{
    public int CommandTimeout { get; init; } = 1200;

    public IsolationLevel? IsolationLevel { get; init; } = null;

    public int? BatchSize { get; init; } = null;
}
