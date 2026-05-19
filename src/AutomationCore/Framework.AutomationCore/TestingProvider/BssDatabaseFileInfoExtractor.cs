using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseFileInfoExtractor(IOptions<AutomationFrameworkSettings> settings) : IDatabaseFileInfoExtractor
{
    public DatabaseFileInfo Extract(TestConnectionString connectionString)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = connectionString.Value };

        var dbPath = Path.Combine(settings.Value.BackupPath, $"{builder.InitialCatalog}.mdf");

        var logPath = Path.Combine(settings.Value.BackupPath, $"{builder.InitialCatalog}_log.ldf");

        return new DatabaseFileInfo(dbPath, logPath);
    }
}
