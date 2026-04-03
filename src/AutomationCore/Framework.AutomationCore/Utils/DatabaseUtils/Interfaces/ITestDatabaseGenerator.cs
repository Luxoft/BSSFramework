namespace Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

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
