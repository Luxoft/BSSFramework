using Automation.Utils.DatabaseUtils.Interfaces;

namespace Automation.Utils.DatabaseUtils;

public interface ITestDatabaseGenerator
{
    IEnumerable<string> TestServers { get; }

    IDatabaseContext DatabaseContext { get; }

    void CreateLocalDb();

    void DeleteLocalDb();

    void DropAllDatabases();

    void ExecuteInsertsForDatabases();

    void GenerateDatabases();

    void DeleteDetachedFiles();

    void CheckAndCreateDetachedFiles();

    void CheckTestDatabase();

    void CheckServerAllowed();

    void GenerateTestData();
}
