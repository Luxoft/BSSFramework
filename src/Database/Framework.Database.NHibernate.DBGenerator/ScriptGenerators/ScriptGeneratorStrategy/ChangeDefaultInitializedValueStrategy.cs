using Anch.Core;

using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.Support;
using Framework.Database.NHibernate.DBGenerator.Team;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Initializes all created columns from the AddedColumns collection that have the <code>VersionAttribute</code> attribute with a default value
/// </summary>
public class ChangeDefaultInitializedValueStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : ScriptGeneratorStrategyBase(parameter)
{
    /// <summary>
    /// Mode of the applied migration script
    /// </summary>
    public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode => ApplyMigrationDbScriptMode.ChangeDefaultValue;

    /// <summary>
    /// Database modifications using a specific strategy
    /// </summary>
    protected override void ExecuteBase()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------Version -----------------------");
        this.ChangeInitializedValueVersionColumns(this.Parameter.Context, this.Parameter.DomainTypesLocal, this.Parameter.AddedColumns);
    }

    private void ChangeInitializedValueVersionColumns(
        IDatabaseScriptGeneratorContext context,
        IEnumerable<DomainTypeMetadata> typeDescriptions,
        IEnumerable<Tuple<Table, Column, string?>> addedColumns)
    {
        var versioningFields = typeDescriptions.GetAllElements(z => z.NotAbstractChildrenDomainTypes).SelectMany(z => z.Fields.Where(f => f.IsVersion));

        var addedVersioningColumns = versioningFields.Join(
            addedColumns,
            z => new { TableName = z.DomainTypeMetadata.DomainType.Name, FieldName = z.Name },
            z => new { TableName = z.Item1.Name, FieldName = z.Item2.Name },
            (domainType, column) => new { Table = column.Item1, Column = column.Item2, DefaultValue = column.Item3 });

        this.Server.ConnectionContext.CapturedSql.Add($"use [{context.GetMainDatabase().Name}]{Environment.NewLine}");

        foreach (var versioningColumn in addedVersioningColumns)
        {
            this.Server.ConnectionContext.CapturedSql.Add(
                $"update {versioningColumn.Table} set [{versioningColumn.Column.Name}]={versioningColumn.DefaultValue}{Environment.NewLine}");
        }

        this.Server.ConnectionContext.CapturedSql.Add(ScriptsHelper.KeywordGo);
    }
}
