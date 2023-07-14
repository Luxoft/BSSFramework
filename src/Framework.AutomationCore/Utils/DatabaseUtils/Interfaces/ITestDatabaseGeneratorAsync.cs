namespace Automation.Utils.DatabaseUtils.Interfaces;

public interface ITestDatabaseGeneratorAsync
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
