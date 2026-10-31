using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;
using Framework.Database.SqlMapper;

using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Base class for generating a part of the database modification script
/// </summary>
public abstract class ScriptGeneratorStrategyBase(DatabaseScriptGeneratorStrategyInfo parameter)
{
    protected readonly DatabaseScriptGeneratorStrategyInfo Parameter = parameter;
    protected readonly Server Server = parameter.Context.GetServer();

    /// <summary>
    /// Mode of the applied migration script
    /// </summary>
    public abstract ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode { get; }

    /// <summary>
    /// Generates an SQL script using the algorithm implemented in a specific strategy
    /// </summary>
    /// <returns>Function returning a collection of SQL commands</returns>
    public Func<IEnumerable<string>> Execute() =>
        () =>
        {
            this.ExecuteStart();
            this.ExecuteBase();
            return this.ExecuteEnd();
        };

    /// <summary>
    /// Database modifications using a specific strategy
    /// </summary>
    protected abstract void ExecuteBase();

    /// <summary>
    /// Searches for a column named <paramref name="sqlMapping.Name"/> in the table <paramref name="table"/>
    /// </summary>
    /// <param name="table">Table in which the column will be searched</param>
    /// <param name="sqlMapping">Column information</param>
    /// <returns>The found column, otherwise null</returns>
    protected Column? GetColumn(Table table, SqlFieldMappingInfo sqlMapping)
    {
        var fieldName = sqlMapping.Name;
        var column = table.Columns.FirstOrDefault(c => c.Name.Equals(fieldName, StringComparison.CurrentCultureIgnoreCase));
        return column;
    }

    private void ExecuteStart()
    {
        this.Server.ConnectionContext.CapturedSql.Clear();
        this.Server.ConnectionContext.SqlExecutionModes = SqlExecutionModes.ExecuteAndCaptureSql;
        this.Server.ConnectionContext.CapturedSql.Add(Environment.NewLine);
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------ START -----------------------");
    }

    private IEnumerable<string> ExecuteEnd()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------ END -----------------------");
        var result = this.Server.ConnectionContext.CapturedSql.GetScriptsForBatchExecuting();
        return result;
    }
}
