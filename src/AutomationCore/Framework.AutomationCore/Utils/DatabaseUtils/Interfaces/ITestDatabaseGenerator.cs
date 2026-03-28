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

    Task GenerateDatabasesAsync();

    void DeleteDetachedFiles();

    Task CheckAndCreateDetachedFilesAsync();

    Task CheckTestDatabaseAsync();

    void CheckServerAllowed();

    Task GenerateTestDataAsync();
}
