namespace Automation.Enums;

public enum TestRunMode
{
    DefaultRunModeOnEmptyDatabase = 0,

    RestoreDatabaseUsingAttach = 1,

    GenerateTestDataOnExistingDatabase = 2,

    RestoreDatabaseFromBackupAndUsingAttach = 3,
}
