namespace Framework.DomainDriven;

public struct DBGeneratorParameters
{
    public DBGeneratorParameters(string serverName, string databaseName)
            : this()
    {
        this.ServerName = serverName;
        this.DatabaseName = databaseName;
    }

    public string DatabaseName { get; private set; }

    public string ServerName { get; private set; }
}
