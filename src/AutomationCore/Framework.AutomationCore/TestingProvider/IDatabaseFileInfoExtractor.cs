using Anch.Testing.Database.ConnectionStringManagement;

namespace Framework.AutomationCore.TestingProvider;

public interface IDatabaseFileInfoExtractor
{
    DatabaseFileInfo Extract(TestConnectionString connectionString);
}
