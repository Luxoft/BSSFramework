using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

namespace SampleSystem.IntegrationTests._Environment;

public abstract class EmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource) : IInitializer
{
    public virtual async Task Initialize(CancellationToken ct)
    {
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync(Path.Combine("__Support", "Scripts", nameof(SampleSystem)), ct);
    }
}
