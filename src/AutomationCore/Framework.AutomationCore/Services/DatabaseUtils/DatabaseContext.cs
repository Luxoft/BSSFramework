using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Services.DatabaseUtils;

public interface IDatabaseContext
{
    TestConnectionString ConnectionString { get; }

    IReadOnlyList<IDatabaseContext> Children { get; }
}

public class DatabaseContext(
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    [FromKeyedServices(nameof(IDatabaseContext.Children))]
    IEnumerable<IDatabaseContext> children) : IDatabaseContext
{
    public TestConnectionString ConnectionString => actualTestConnectionStringSource.ActualConnectionString;

    public IReadOnlyList<IDatabaseContext> Children { get; } = children.ToList();
}
