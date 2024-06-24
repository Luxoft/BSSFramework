using Automation.Enums;

namespace Automation.Settings;

public class AutomationFrameworkSettings
{
    public string IntegrationTestUserName { get; set; } = "IntegrationTestRootUser";

    public TestRunMode TestRunMode { get; set; } = TestRunMode.DefaultRunModeOnEmptyDatabase;

    public bool UseLocalDb { get; set; }

    public bool TestsParallelize { get; set; }

    public string DatabaseCollation { get; set; } = "";

    public string TestRunServerRootFolder { get; set; }

    public string DbDataDirectory => Path.Combine(this.TestRunServerRootFolder, "data");

    public string TempFolder => Path.Combine(this.TestRunServerRootFolder, "temp");

    public string ConnectionStringName { get; set; } = "DefaultConnection";

    public string[] SecondaryDatabases { get; set; } = Array.Empty<string>();
}
