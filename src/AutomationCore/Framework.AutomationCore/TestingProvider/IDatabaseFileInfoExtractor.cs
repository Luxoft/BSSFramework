using Anch.Testing.Database.ConnectionStringManagement;

namespace Framework.AutomationCore.TestingProvider;

public interface IDatabaseFileInfoExtractor
{
    MsSqlDatabaseFileInfo Extract(TestConnectionString connectionString);
}
