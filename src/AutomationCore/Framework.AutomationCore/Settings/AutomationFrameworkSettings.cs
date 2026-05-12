using Anch.Testing.Database;

namespace Framework.AutomationCore.Settings;

public class AutomationFrameworkSettings
{
    public string IntegrationTestUserName { get; set; } = "IntegrationTestRootUser";

    public DatabaseInitMode DatabaseInitMode { get; set; } = DatabaseInitMode.RebuildSnapshot;

    public bool UseLocalDb { get; set; }

    public bool TestsParallelize { get; set; }

    public string DatabaseCollation { get; set; } = "";

    public string BackupPath { get; set; } = null!;

    public string[] SecondaryDatabases { get; set; } = [];

    public string[] LocalAdmins { get; set; } = [];
}
