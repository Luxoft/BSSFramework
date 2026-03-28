using CommonFramework;

using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;
using Framework.Database.SqlMapper;

using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Удаляет колонки лежащие в RemovableColumns
/// </summary>
/// <remarks>Если установлен флаг DatabaseScriptGeneratorMode.RemoveObsoleteColumns, то удаляет все колонки имен которых нету среди названий полей соответствующего доменного типа</remarks>
internal class RemoveStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : ScriptGeneratorStrategyBase(parameter)
{
    /// <summary>
    /// Мод применяемого миграционого скрипта
    /// </summary>
    public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode
    {
        get
        {
            return ApplyMigrationDbScriptMode.Remove;
        }
    }

    /// <summary>
    /// Модификации базы данных по определенной стратегии
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

    private void RemoveColumns(Column removableColumn)
    {
        this.RemoveColumns(removableColumn.Parent as Table, [removableColumn]);
    }

    private void RemoveColumns(Table table, IEnumerable<Column> removableColums)
    {
        var allFk =
                table.ForeignKeys.SelectMany(
                                                                z => z.Columns.Select(c => new { Fk = z, Column = c }));

        var removeFk = allFk.Join(removableColums, c => c.Column.Name, c => c.Name, (c1, c2) => c1).ToList();

        removeFk.Foreach(z => z.Fk.Drop());

        var allIndexes = table.Indexes.SelectMany(z => z.IndexedColumns.Select(c => new { Index = z, Column = c }));

        var removeIndexes = allIndexes.Join(removableColums, c => c.Column.Name, c => c.Name, (c1, c2) => c1).ToList();
        removeIndexes.Foreach(z => z.Index.Drop());

        removableColums.Where(z => null != z.DefaultConstraint).Foreach(
                                                                        z =>
                                                                        {
                                                                            z.UnbindDefault();
                                                                            z.Alter();
                                                                        });

        removableColums.Foreach(z => z.Drop());
    }

    private void TryRemoveNotExistColumns(DomainTypeMetadata typeDescription)
    {
        var table = this.Parameter.Context.GetOrCreateTable(typeDescription.DomainType);
        var sqlMappings = typeDescription.Fields.SelectMany(MapperFactory.GetMapping).ToList();

        if (typeDescription.Parent != null)
        {
            sqlMappings = sqlMappings.Union(typeDescription.Root.Fields.SelectMany(MapperFactory.GetMapping).Where(f => f.IsPrimaryKey))
                                     .ToList();
        }

        var columns = table.Columns.Cast<Column>();

        var removableColums = columns
                              .Where(z => null == sqlMappings.FirstOrDefault(w => w.Name.Equals(z.Name, StringComparison.CurrentCultureIgnoreCase)))
                              .ToList();

        this.RemoveColumns(table, removableColums);
    }
}
