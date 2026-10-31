using Anch.Core;

using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;
using Framework.Database.SqlMapper;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Removes the columns in RemovableColumns
/// </summary>
/// <remarks>If the DatabaseScriptGeneratorMode.RemoveObsoleteColumns flag is set, removes all columns whose names are not among the field names of the corresponding domain type</remarks>
internal class RemoveStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : ScriptGeneratorStrategyBase(parameter)
{
    /// <summary>
    /// Mode of the applied migration script
    /// </summary>
    public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode => ApplyMigrationDbScriptMode.Remove;

    /// <summary>
    /// Database modifications using a specific strategy
    /// </summary>
    protected override void ExecuteBase()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------RemoveColumns-----------------------");
        foreach (var removableColumn in this.Parameter.RemovableColumns)
        {
            this.RemoveColumns(removableColumn);
        }

        if (this.Parameter.DatabaseGeneratorMode.HasFlag(DatabaseScriptGeneratorMode.RemoveObsoleteColumns))
        {
            this.Server.ConnectionContext.CapturedSql.Add("------------------------------TryRemoveNotExistColumns-----------------------");
            foreach (var domainTypeMetadata in this.Parameter.DomainTypesLocal)
            {
                this.TryRemoveNotExistColumns(domainTypeMetadata);
            }
        }
    }

    private void RemoveColumns(Column removableColumn) => this.RemoveColumns((removableColumn.Parent as Table)!, [removableColumn]);

    private void RemoveColumns(Table table, IEnumerable<Column> removableColumns)
    {
        var allFk =
                table.ForeignKeys.SelectMany(
                                                                z => z.Columns.Select(c => new { Fk = z, Column = c }));

        var removeFk = allFk.Join(removableColumns, c => c.Column.Name, c => c.Name, (c1, c2) => c1).ToList();

        removeFk.Foreach(z => z.Fk.Drop());

        var allIndexes = table.Indexes.SelectMany(z => z.IndexedColumns.Select(c => new { Index = z, Column = c }));

        var removeIndexes = allIndexes.Join(removableColumns, c => c.Column.Name, c => c.Name, (c1, c2) => c1).ToList();
        removeIndexes.Foreach(z => z.Index.Drop());

        removableColumns.Where(z => null != z.DefaultConstraint).Foreach(
                                                                        z =>
                                                                        {
                                                                            z.UnbindDefault();
                                                                            z.Alter();
                                                                        });

        removableColumns.Foreach(z => z.Drop());
    }

    private void TryRemoveNotExistColumns(DomainTypeMetadata typeDescription)
    {
        var table = this.Parameter.Context.GetOrCreateTable(typeDescription.DomainType);
        var sqlMappings = typeDescription.Fields.SelectMany(MapperFactory.GetMapping).ToList();

        if (typeDescription.Parent is not null)
        {
            sqlMappings = sqlMappings.Union(typeDescription.Root.Fields.SelectMany(MapperFactory.GetMapping).Where(f => f.IsPrimaryKey))
                                     .ToList();
        }

        var columns = table.Columns.Cast<Column>();

        var removableColumns = columns
                              .Where(z => null == sqlMappings.FirstOrDefault(w => w.Name.Equals(z.Name, StringComparison.CurrentCultureIgnoreCase)))
                              .ToList();

        this.RemoveColumns(table, removableColumns);
    }
}
