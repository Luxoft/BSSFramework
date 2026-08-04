using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.Initializers;
using Anch.Testing.Database.Mssql.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Anch.Testing.Database.Mssql;

public class MssqlEmptySchemaInitializer(
    [FromKeyedServices(TestDatabaseInitializer.EmptySchemaKey)] IInitializer emptySchemaInitializer,
    IActualTestConnectionStringSource actualTestConnectionStringSource,
    IEnumerable<ExecuteScriptInfo> scriptInfoList) : IInitializer
{
    public const string Key = nameof(MssqlEmptySchemaInitializer);

    public async Task Initialize(CancellationToken ct)
    {
        await emptySchemaInitializer.Initialize(ct);

        foreach (var executeScriptInfo in scriptInfoList)
        {
            await actualTestConnectionStringSource.ActualConnectionString.ExecuteSqlFromFolderAsync(executeScriptInfo.Path, ct);
        }
    }
}
