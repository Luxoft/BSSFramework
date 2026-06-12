using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Initializers;

using Framework.AutomationCore.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Services;

public class BssEmptySchemaInitializer(
    [FromKeyedServices(TestDatabaseInitializer.EmptySchemaKey)] IInitializer emptySchemaInitializer,
    IActualTestConnectionStringSource actualTestConnectionStringSource, IEnumerable<ExecuteScriptInfo> scriptInfoList) : IInitializer
{
    public const string Key = nameof(BssEmptySchemaInitializer);

    public async Task Initialize(CancellationToken ct)
    {
        await emptySchemaInitializer.Initialize(ct);

        foreach (var executeScriptInfo in scriptInfoList)
        {
            await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync(executeScriptInfo.Path, ct);
        }
    }
}

