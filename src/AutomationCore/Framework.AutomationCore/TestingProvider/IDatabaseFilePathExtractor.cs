using Anch.Testing.Database.ConnectionStringManagement;

namespace Framework.AutomationCore.TestingProvider;

public interface IDatabaseFilePathExtractor
{
    string Extract(TestConnectionString connectionString);
}
