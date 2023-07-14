namespace Automation.Utils.DatabaseUtils;

public class DatabaseContextSettings
{
    public string ConnectionStringName { get; set; } = "DefaultConnection";

    public string[] SecondaryDatabases { get; set; } = Array.Empty<string>();

    public DatabaseContextSettings()
    {
    }

    public DatabaseContextSettings(string connectionStringName, string[] secondaryDatabases)
    {
        this.ConnectionStringName = connectionStringName;
        this.SecondaryDatabases = secondaryDatabases;
    }
}
