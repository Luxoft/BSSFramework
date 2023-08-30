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