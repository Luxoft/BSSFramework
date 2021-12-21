namespace Framework.DomainDriven.DBGenerator.Team
{
    public struct ExecutingScriptTable
    {
        public string DatabaseName { get; private set; }
        public string TableName { get; private set; }

        public ExecutingScriptTable(string databaseName, string tableName) : this()
        {
            this.DatabaseName = databaseName;
            this.TableName = tableName;
        }
    }
}