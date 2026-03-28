namespace Framework.Database.NHibernate.DBGenerator;

public struct DBGeneratorParameters(string serverName, string databaseName)
{
    public string DatabaseName { get; private set; } = databaseName;

    public string ServerName { get; private set; } = serverName;
}
