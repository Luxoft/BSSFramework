using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;

/// <summary>
/// Создает Index, PrimaryKey и ForeignKey, если их еще нету, для колонок всех доменных типов. Если в доменом объекте полей не должны принимать значниея null, то колонка также будет не Nullable
/// </summary>
internal class ChangeIndexesStrategy : ScriptGeneratorStrategyBase
{
    public ChangeIndexesStrategy(DatabaseScriptGeneratorStrategyInfo parameter)
            : base(parameter)
    {
    }

    /// <summary>
    /// Мод применяемого миграционого скрипта
    /// </summary>
    public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode
    {
        get
        {
            return ApplyMigrationDbScriptMode.ChangeIndexies;
        }
    }

    private static bool IsSameByName(Index index, string indexName)
    {
        return index.Name.Equals(indexName, StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsSameByColumns(Index index, IndexKeyType newIndexKeyType, IEnumerable<string> columnNames)
    {
        var isMoreSpecific = index.IndexKeyType == IndexKeyType.DriUniqueKey && newIndexKeyType == IndexKeyType.None;
        var isSameType = index.IndexKeyType == newIndexKeyType;

        return (isMoreSpecific || isSameType) && GetExistingIndexColumns(index).SequenceEqual(columnNames);
    }

    private static IOrderedEnumerable<string> GetExistingIndexColumns(Index index)
    {
        return index.IndexedColumns.Cast<IndexedColumn>()
                    .Where(x => !x.IsIncluded)
                    .Select(column => column.Name)
                    .OrderBy(q => q);
    }

    private static void CreateNewIndex(Table table, string indexName, IndexKeyType indexKeyType, IEnumerable<string> columnNames)
    {
        var index = new Index(table, indexName)
                    {
                            IndexKeyType = indexKeyType
                    };

        columnNames.Foreach(z => index.IndexedColumns.Add(new IndexedColumn(index, z)));

        table.Indexes.Add(index);
        index.Create();
    }

    /// <summary>
    /// Модификации базы данных по определенной стратегии
    /// </summary>
    protected override void ExecuteBase()
    {
        this.Server.ConnectionContext.CapturedSql.Add("------------------------------START Set not null columns-----------------------");

        foreach (var typeDescription in this.Parameter.DomainTypesLocal)
        {
            this.SetNotNullColumns(typeDescription);
        }

        this.Server.ConnectionContext.CapturedSql.Add(ScriptsHelper.KeywordGo);

        this.Server.ConnectionContext.CapturedSql.Add("------------------------------END Set not null columns-----------------------");

        this.Server.ConnectionContext.CapturedSql.Add("------------------------------START ChangeIndexies-----------------------");

        this.Server.ConnectionContext.CapturedSql.Add("------------------------------ChangeIndexesForRemovableColumns-----------------------");
        foreach (var domainTypeMetadata in this.Parameter.DomainTypesLocal)
        {
            this.ChangeIndexesForRemovableColumns(domainTypeMetadata);
        }

        this.Server.ConnectionContext.CapturedSql.Add("------------------------------GenerateIndexies-----------------------");
        foreach (var typeMetadata in this.Parameter.DomainTypesLocal)
        {
            this.GenerateIndexes(typeMetadata);
        }

        this.Server.ConnectionContext.CapturedSql.Add("------------------------------GenerateFksForDomainType-----------------------");
        foreach (var typeDescription in this.Parameter.DomainTypesLocal)
        {
            this.GenerateFksForDomainType(typeDescription);
        }
    }

    private void ChangeIndexesForRemovableColumns(DomainTypeMetadata domainTypeMetadata)
    {
        var table = this.Parameter.Context.GetOrCreateTable(domainTypeMetadata.DomainType);
        var removableColumns = this.Parameter.RemovableColumns.Where(z => z.Parent == table).ToList();

        var previusIndexies = table.Indexes.Cast<Index>()
                                   .Where(z => z.IndexedColumns.Cast<IndexedColumn>().Any(q => q.Name.EndsWith(this.Parameter.PreviusPostfix)))
                                   .ToList();

        foreach (var previousIndex in previusIndexies)
        {
            var newIndex = new Index(table, previousIndex.Name);

            newIndex.IndexKeyType = previousIndex.IndexKeyType;
            newIndex.IsUnique = previousIndex.IsUnique;

            foreach (var column in previousIndex.IndexedColumns.Cast<IndexedColumn>())
            {
                var endIndex = column.Name.IndexOf(this.Parameter.PreviusPostfix);
                var actualColumnName = endIndex == -1
                                               ? column.Name
                                               : new string(column.Name.Take(endIndex).ToArray());
                var newIndexColumn = new IndexedColumn(newIndex, actualColumnName);
                newIndex.IndexedColumns.Add(newIndexColumn);
            }

            previousIndex.Drop();

            newIndex.Create();
        }

        foreach (var removableColumn in removableColumns)
        {
            var indexedColumn = table.Indexes.Cast<Index>().SelectMany(
                                                                       z => z.IndexedColumns.Cast<IndexedColumn>().Where(q => q.Name == removableColumn.Name))
                                     .ToList();

            foreach (var index in indexedColumn.GroupBy(z => z.Parent))
            {
                index.Key.Drop();

                table.Indexes.Remove(index.Key);
            }

            foreach (var value in indexedColumn)
            {
                var oldNameIndex = value.Name.IndexOf(this.Parameter.PreviusPostfix);
                var index = value.Parent;

                index.IndexedColumns.Remove(value);
                var newName = new string(removableColumn.Name.Take(oldNameIndex).ToArray());
                index.IndexedColumns.Add(new IndexedColumn(value.Parent, newName));
                index.IndexedColumns.Remove(value);

                value.Parent.Alter();
            }
        }
    }

    private void SetNotNullColumns(DomainTypeMetadata typeDescription)
    {
        var table = this.Parameter.Context.GetTable(typeDescription.DomainType);

        this.SetNotNullColumns(typeDescription, table);

        foreach (var childDomainType in typeDescription.NotAbstractChildrenDomainTypes)
        {
            this.SetNotNullColumns(childDomainType);
        }
    }

    private void SetNotNullColumns(DomainTypeMetadata typeDescription, Table table)
    {
        var sqlMappings = typeDescription.Fields.SelectMany(MapperFactory.GetMapping).ToList();

        if (typeDescription.Parent != null)
        {
            sqlMappings = sqlMappings.Union(typeDescription.Root.Fields.SelectMany(MapperFactory.GetMapping).Where(f => f.IsPrimaryKey))
                                     .ToList();
        }

        foreach (var sqlMapping in sqlMappings.OrderBy(z => z.Name).Where(z => !z.IsNullable))
        {
            var column = this.GetColumn(table, sqlMapping);

            if (!column.Nullable)
            {
                continue;
            }

            column.Nullable = false;
            column.Alter();
        }
    }

    private void GenerateIndexes(DomainTypeMetadata typeDescription)
    {
        var table = this.Parameter.Context.GetOrCreateTable(typeDescription.DomainType);
        this.GenerateIndexes(typeDescription, table);

        foreach (var childDomainType in typeDescription.NotAbstractChildrenDomainTypes)
        {
            this.GenerateIndexes(childDomainType);
        }
    }

    private void GenerateIndexes(DomainTypeMetadata domainTypeMetadata, Table table)
    {
        this.GeneratePrimaryKey(domainTypeMetadata, table);

        var sqlMappings = domainTypeMetadata.GetPersistentReferenceFields().SelectMany(MapperFactory.GetMapping)
                                            .Concat(domainTypeMetadata.PrimitiveFields.SelectMany(MapperFactory.GetMapping).Where(z => z.IsUniqueKey));

        foreach (var mappingInfo in sqlMappings)
        {
            var indexName = $"IX_{table.Name}_{mappingInfo.Name}";
            this.TryCreateIndex(table, indexName, mappingInfo.IsUniqueKey ? IndexKeyType.DriUniqueKey : IndexKeyType.None, new[] { mappingInfo.Name });
        }
    }

    private void GeneratePrimaryKey(DomainTypeMetadata domainTypeMetadata, Table table)
    {
        var sqlMappings = domainTypeMetadata.Root.PrimitiveFields.SelectMany(MapperFactory.GetMapping);
        var primaryKeyColumns = sqlMappings.Where(z => z.IsPrimaryKey);
        this.TryCreateIndex(table, "PK_" + table.Name, IndexKeyType.DriPrimaryKey, primaryKeyColumns.Select(z => z.Name));
    }

    private void TryCreateIndex(Table table, string indexName, IndexKeyType indexKeyType, IEnumerable<string> columnNames)
    {
        if (!this.Parameter.IgnoredIndexes.Contains(indexName))
        {
            columnNames = columnNames.OrderBy(z => z).ToList();

            var sameIndexes = table.Indexes.Cast<Index>()
                                   .Where(index => IsSameByName(index, indexName)
                                                   || IsSameByColumns(index, indexKeyType, columnNames));

            if (!sameIndexes.Any())
            {
                CreateNewIndex(table, indexName, indexKeyType, columnNames);
            }
        }
    }

    private void GenerateFksForDomainType(DomainTypeMetadata typeDescription)
    {
        var table = this.Parameter.Context.GetOrCreateTable(typeDescription.DomainType);
        if (this.GenerateForeignKeys(typeDescription, table))
        {
            foreach (var childDomainType in typeDescription.NotAbstractChildrenDomainTypes)
            {
                this.GenerateFksForDomainType(childDomainType);
            }
        }
    }

    private bool GenerateForeignKeys(DomainTypeMetadata domainTypeMetadata, Table table)
    {
        var result = false;
        var dict = this.Parameter.TypeToDomainTypeMetadataDictionary;
        foreach (var reference in domainTypeMetadata.GetPersistentReferenceFields().SelectMany(MapperFactory.GetMapping))
        {
            var referenceTypeFieldMetadata = (ReferenceTypeFieldMetadata)reference.Source;

            if (dict.ContainsKey(referenceTypeFieldMetadata.ToType) && dict[referenceTypeFieldMetadata.ToType].IsView)
            {
                continue;
            }


            var foreignKeyName = ((ReferenceTypeFieldMetadata)reference.Source).GetForeignKeyName();
            var existingForeignKey = table.ForeignKeys.Cast<ForeignKey>().FirstOrDefault(z => z.Name.Equals(foreignKeyName, StringComparison.CurrentCultureIgnoreCase));
            if (null != existingForeignKey)
            {
                continue;
            }

            var toTable = this.Parameter.Context.GetOrCreateTable(reference.Source.Type);
            var foreignKey = new ForeignKey(table, foreignKeyName);
            foreignKey.ReferencedTable = toTable.Name;
            foreignKey.ReferencedTableSchema = toTable.Schema;

            var primeryKeyColumnName = toTable
                                       .Indexes.Cast<Index>().Single(z => z.IndexKeyType == IndexKeyType.DriPrimaryKey)
                                       .IndexedColumns.Cast<IndexedColumn>()
                                       .First().Name;

            foreignKey.Columns.Add(new ForeignKeyColumn(foreignKey, reference.Name, primeryKeyColumnName));
            table.ForeignKeys.Add(foreignKey);

            foreignKey.Create();

            result = true;
        }

        if (null != domainTypeMetadata.Parent)
        {
            var toTable = this.Parameter.Context.GetOrCreateTable(domainTypeMetadata.Parent.DomainType);

            var foreignKeyName = "FK_" + table.Name + "_" + toTable.Name;
            var existingForeignKey = table.ForeignKeys.Cast<ForeignKey>().FirstOrDefault(z => z.Name.Equals(foreignKeyName, StringComparison.CurrentCultureIgnoreCase));
            if (null == existingForeignKey)
            {
                var foreignKey = new ForeignKey(table, foreignKeyName);
                foreignKey.ReferencedTable = toTable.Name;
                foreignKey.ReferencedTableSchema = toTable.Schema;
                foreignKey.Columns.Add(new ForeignKeyColumn(foreignKey, "id", "id"));
                table.ForeignKeys.Add(foreignKey);

                foreignKey.Create();

                result = true;
            }
        }

        return result;
    }
}
