using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy
{
    /// <summary>
    /// Удаляет колонки лежащие в RemovableColumns
    /// </summary>
    /// <remarks>Если установлен флаг DatabaseScriptGeneratorMode.RemoveObsoleteColumns, то удаляет все колонки имен которых нету среди названий полей соответствующего доменного типа</remarks>
    internal class RemoveStrategy : ScriptGeneratorStrategyBase
    {
        public RemoveStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : base(parameter)
        {
        }

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
            this.RemoveColumns(removableColumn.Parent as Table, new[] { removableColumn });
        }

        private void RemoveColumns(Table table, IEnumerable<Column> removableColums)
        {
            var allFK =
                table.ForeignKeys.Cast<ForeignKey>().SelectMany(
                    z => z.Columns.Cast<ForeignKeyColumn>().Select(c => new { Fk = z, Column = c }));

            var removeFK = allFK.Join(removableColums, c => c.Column.Name, c => c.Name, (c1, c2) => c1).ToList();

            removeFK.Foreach(z => z.Fk.Drop());

            var allIndexes = table.Indexes.Cast<Index>().SelectMany(z => z.IndexedColumns.Cast<IndexedColumn>().Select(c => new { Index = z, Column = c }));

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
}
