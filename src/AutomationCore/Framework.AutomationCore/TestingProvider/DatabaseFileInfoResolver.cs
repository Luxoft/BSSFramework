using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.TestingProvider;

public class DatabaseFileInfoResolver(IOptions<AutomationFrameworkSettings> settings) : IDatabaseFileInfoResolver
{
    public DatabaseFileInfo Resolve(string initialCatalog)
    {
        var dbPath = Path.Combine(settings.Value.BackupPath, $"{initialCatalog}.mdf");

        var logPath = Path.Combine(settings.Value.BackupPath, $"{initialCatalog}_log.ldf");

        return new DatabaseFileInfo(dbPath, logPath);
    }
}
