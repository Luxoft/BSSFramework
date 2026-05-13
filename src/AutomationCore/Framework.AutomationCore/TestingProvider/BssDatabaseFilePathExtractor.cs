using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Settings;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseFilePathExtractor(IOptions<AutomationFrameworkSettings> settings) : IDatabaseFilePathExtractor
{
    public string Extract(TestConnectionString connectionString)
    {
        var builder = new SqlConnectionStringBuilder { ConnectionString = connectionString.Value };

        return Path.Combine(settings.Value.BackupPath, $"{builder.InitialCatalog}.mdf");
    }
}
