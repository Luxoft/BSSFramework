using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Extensions;

using SampleSystem.DbGenerate;
using SampleSystem.IntegrationTests._Environment.FluentMigration;

namespace SampleSystem.IntegrationTests._Environment;

public abstract class EmptySchemaInitializer(IActualTestConnectionStringSource actualTestConnectionStringSource) : IInitializer
{
    public virtual async Task Initialize(CancellationToken ct)
    {
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Authorization", ct);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/Configuration", ct);
        await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync("__Support/Scripts/SampleSystem", ct);

        new BssFluentMigrator(actualTestConnectionStringSource.ActualConnectionString.Value, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
    }
}

