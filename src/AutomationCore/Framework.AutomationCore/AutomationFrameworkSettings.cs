using Anch.Testing.Database;

namespace Framework.AutomationCore;

public class AutomationFrameworkSettings
{
    public string IntegrationTestUserName { get; set; } = "IntegrationTestRootUser";

    public DatabaseInitMode DatabaseInitMode { get; set; } = DatabaseInitMode.RebuildSnapshot;

    public bool TestsParallelize { get; set; } = true;

    public string DatabaseCollation { get; set; } = "";

    public string BackupPath { get; set; } = "";

    public string[] SecondaryDatabases { get; set; } = [];

    public string[] LocalAdmins { get; set; } = [];
}
