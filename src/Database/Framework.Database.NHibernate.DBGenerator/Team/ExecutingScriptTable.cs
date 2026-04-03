namespace Framework.Database.NHibernate.DBGenerator.Team;

public struct ExecutingScriptTable(string databaseName, string tableName)
{
    public string DatabaseName { get; private set; } = databaseName;

    public string TableName { get; private set; } = tableName;
}
