# Integration Tests

### Parameters:
|                    Name | Description                                                                                                                                                                                            | Default Value                 | Required | 
|------------------------:|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------|----------|
|              SystemName | System name, used to name the database files                                                                                                                                                           | ""                            | true     |
|             TestRunMode | Test Run Mode                                                                                                                                                                                          | DefaultRunModeOnEmptyDatabase | false    |
|              UseLocalDb | Flag defining the database used for running tests (MSSQL LocalDB or MSSQL Server)                                                                                                                      | false                         | false    |
|      TestsParallelize   | A flag that determines whether or not to allow parallel execution of tests (configured with the corresponding MSTest/xUnit/NUnit attributes). Additionally enables randomization of the database name. | false                         | false    |
| TestRunServerRootFolder | Path to the folder to store temporary files and generated database files                                                                                                                               | ""                            | true     |
|       ConnectionStrings | Collection of database connection strings                                                                                                                                                              | []                            | true     |

### TestRunMode available values:
- DefaultRunModeOnEmptyDatabase: The database is generated once before running all tests, existing database files are deleted. After the tests are done, all databases are deleted.
- RestoreDatabaseUsingAttach: If there is a generated database, existing files will be used. Otherwise, a new one will be generated before running all tests. After the tests are completed, the files and databases will not be deleted.
- GenerateTestDataOnExistingDatabase: The test is performed on the existing base, without performing a new base generation / database deletion after the test.


# Integration Tests Xunit
 - Implement IAutomationCoreInitialization somewhere in your project.
 - Add [assembly: TestFramework("Automation.Xunit.AutomationCoreTestFramework", "Framework.AutomationCore.Xunit")]
 - xUnit [Theory] should be replaced by [AutomationCoreTheory] in order to use ability to initialize tests via injected ITestInitializeAndCleanup/ITestInitializeAndCleanupAsync instead of Fixture/TestBase implementation
Example:
```
[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass)]
[assembly: TestFramework("Automation.Xunit.AutomationCoreTestFramework", "Framework.AutomationCore.Xunit")]

public class EnvironmentInitialization : IAutomationCoreInitialization
{
    public IServiceCollection ConfigureFramework(IServiceCollection services) =>
        services
            .AddSingleton<IAssemblyInitializeAndCleanupAsync, DiAssemblyInitializeAndCleanupAsync>()
            .AddSingleton<ITestDatabaseGeneratorAsync, DatabaseGenerator>()
            .AddSingleton<IConfiguration>(
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .AddEnvironmentVariables("LS_")
                    .Build());

    public IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration) =>
        services
            .AddApplication(configuration, new HostingEnvironment { EnvironmentName = Environments.Development })
            .AddIntegrationTestServices(
                options =>
                {
                    options.IntegrationTestUserName = TestConstants.Principals.Admin.Name;
                })
            .BuildServiceProvider();
}
```