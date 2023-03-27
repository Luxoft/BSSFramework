using Framework.Core;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Проинициализирует все соданные колонки, из коллекции AddedColumns, с атрибутом <code>VersionAttribute</code> значением по умолчанию
/// </summary>
internal class ChangeDefaultInitializedValueStrategy : ScriptGeneratorStrategyBase
{
    public ChangeDefaultInitializedValueStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : base(parameter)
    {
    }

    /// <summary>
    /// Мод применяемого миграционого скрипта
    /// </summary>
    public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode
    {
        get
        {
            return ApplyMigrationDbScriptMode.ChangeDefaultValue;
        }
    }

    /// <summary>
    /// Модификации базы данных по определенной стратегии
    /// </summary>
    protected override void ExecuteBase()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------Version -----------------------");
        this.ChangeInitializedValueVersionColumns(this.Parameter.Context, this.Parameter.DomainTypesLocal, this.Parameter.AddedColumns);
    }

    private void ChangeInitializedValueVersionColumns(IDatabaseScriptGeneratorContext context, IEnumerable<DomainTypeMetadata> typeDescriptions, IEnumerable<Tuple<Table, Column, string>> addedColumns)
    {
        var versioningFields = typeDescriptions.GetAllElements(z => z.NotAbstractChildrenDomainTypes).SelectMany(z => z.Fields.Where(f => f.IsVersion));

        var addedVersioningColumns = versioningFields.Join(
                                                           addedColumns,
                                                           z => new
                                                                {
                                                                        TableName = z.DomainTypeMetadata.DomainType.Name,
                                                                        FieldName = z.Name
                                                                },
                                                           z => new
                                                                {
                                                                        TableName = z.Item1.Name,
                                                                        FieldName = z.Item2.Name
                                                                },
                                                           (domainType, column) => new
                                                                                   {
                                                                                           Table = column.Item1,
                                                                                           Column = column.Item2,
                                                                                           DefaultValue = column.Item3
                                                                                   });

        this.Server.ConnectionContext.CapturedSql.Add($"use [{context.GetMainDatabase().Name}]{Environment.NewLine}");

        foreach (var versioningColumn in addedVersioningColumns)
        {
            this.Server.ConnectionContext.CapturedSql.Add(
                                                          $"update {versioningColumn.Table} set [{versioningColumn.Column.Name}]={versioningColumn.DefaultValue}{Environment.NewLine}");
        }

        this.Server.ConnectionContext.CapturedSql.Add(ScriptsHelper.KeywordGo);
    }
}
