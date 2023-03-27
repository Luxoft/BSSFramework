using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.Persistent.Mapping;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators;

public static class DatabaseScriptGeneratorContextExtension
{
    public static Database GetMainDatabase(this IDatabaseScriptGeneratorContext context)
    {
        return context.SqlDatabaseFactory.GetOrCreateDatabase(context.DatabaseName);
    }

    public static Server GetServer(this IDatabaseScriptGeneratorContext context)
    {
        return context.SqlDatabaseFactory.Server;
    }

    public static Table GetTable(this IDatabaseScriptGeneratorContext context, Type type)
    {
        var database = context.GetMainDatabase();

        var name = type.GetTableName(context.DatabaseName.Schema);

        var result = TryGetTable(context, database, name.tableName, name.schemaName);

        if (null == result)
        {
            throw new ArgumentException($"Expected table:'{name}' for type:'{type.Name}'");
        }

        return result;
    }

    private static Table TryGetTable(IDatabaseScriptGeneratorContext context, Database database, string name, string schema)
    {
        var result = database.Tables.Cast<Table>().FirstOrDefault(q =>
                                                                          string.Equals(q.Name, name, StringComparison.InvariantCultureIgnoreCase)
                                                                          && string.Equals(q.Schema, schema, StringComparison.InvariantCultureIgnoreCase));
        return result;
    }

    public static Table GetOrCreateTable(this IDatabaseScriptGeneratorContext context, Type type)
    {
        var tableNameInfo = type.GetTableName(context.DatabaseName.Schema);
        return context.GetOrCreateTable(tableNameInfo.tableName, tableNameInfo.schemaName);
    }

    private static Table GetOrCreateTable(this IDatabaseScriptGeneratorContext context, string name, string schema)
    {
        var database = context.GetMainDatabase();

        var result = TryGetTable(context, database, name, schema);

        if (null == result)
        {
            result = new Table(database, name, context.DatabaseName.Schema);

            database.Tables.Add(result);
        }

        return result;
    }
}
