using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using Framework.Core;
using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy
{
    /// <summary>
    /// Создает таблицы и колонки в этих таблицах для всех доменных типов
    /// </summary>
    /// <remarks>Если установлен флаг DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript и в доменном объекте поменялся тип колонки, то колонка будет переименовано с постфиксом '_previusVersion' и в скрипт допишется строка update {0} set [{1}]=[{2}].
    /// Созданные колонки добавляются в parameter.AddedColumns</remarks>
    internal class AddOrUpdateStrategy : ScriptGeneratorStrategyBase
    {
        public AddOrUpdateStrategy(DatabaseScriptGeneratorStrategyInfo parameter) : base(parameter)
        {
        }

        /// <summary>
        /// Мод применяемого миграционого скрипта
        /// </summary>
        public override ApplyMigrationDbScriptMode ApplyMigrationDbScriptMode
        {
            get
            {
                return ApplyMigrationDbScriptMode.AddOrUpdate;
            }
        }

        /// <summary>
        /// Модификации базы данных по определенной стратегии
        /// </summary>
        protected override void ExecuteBase()
        {
            var mainDatabase = this.Parameter.Context.GetMainDatabase();

            mainDatabase.Tables.Refresh();

            foreach (var typeDescription in this.Parameter.DomainTypesLocal)
            {
                this.GenerateTableForDomainType(typeDescription);
            }
        }

        private void GenerateTableForDomainType(DomainTypeMetadata typeDescription)
        {
            var table = this.Parameter.Context.GetOrCreateTable(typeDescription.DomainType);

            this.GenerateColumns(typeDescription, table);

            foreach (var childDomainType in typeDescription.NotAbstractChildrenDomainTypes)
            {
                this.GenerateTableForDomainType(childDomainType);
            }
        }

        private void GenerateColumns(DomainTypeMetadata typeDescription, Table table)
        {
            var sqlMappings = typeDescription.GetPersistentFields().SelectMany(MapperFactory.GetMapping).ToList();
            var mainDatabase = this.Parameter.Context.GetMainDatabase();

            if (typeDescription.Parent != null)
            {
                sqlMappings = sqlMappings.Union(typeDescription.Root.Fields.SelectMany(MapperFactory.GetMapping).Where(f => f.IsPrimaryKey))
                    .ToList();
            }

            foreach (var sqlMapping in sqlMappings.OrderBy(z => z.Name))
            {
                var column = this.GetOrCreateColumn(table, sqlMapping);

                if (!this.Parameter.DataTypeComparer.Equals(column.DataType, sqlMapping.SqlType))
                {
                    var columnName = sqlMapping.Name;
                    var columnType = sqlMapping.SqlType;

                    if (null != column.DefaultConstraint)
                    {
                        column.UnbindDefault();
                    }

                    column.Rename(column.Name + this.Parameter.PreviusPostfix);

                    column.Alter();

                    this.Parameter.RemovableColumns.Add(column);

                    var newColumn = this.CreateColumn(table, columnName, columnType, sqlMapping.IsNullable, sqlMapping.DefaultConstraint);

                    // TODO: move this if to custom script
                    if (this.Parameter.DatabaseGeneratorMode.HasFlag(DatabaseScriptGeneratorMode.AutoGenerateUpdateChangeTypeScript))
                    {
                        this.Server.ConnectionContext.CapturedSql.Add(ScriptsHelper.KeywordGo);
                        var script = new StringCollection
                                         {
                                             $"use [{mainDatabase.Name}]",
                                             $"update {table.ToString()} set [{newColumn.Name}]=[{column.Name}]"
                                         };

                        this.Server.ConnectionContext.ExecuteNonQuery(script);
                    }

                    if (column.Nullable != sqlMapping.IsNullable && !this.Parameter.DataTypeComparer.Equals(column.DataType, sqlMapping.SqlType))
                    {
                        column.Alter();

                        table.ForeignKeys.Cast<ForeignKey>()
                            .Where(z => z.Columns.Cast<ForeignKeyColumn>().Any(q => q.Name == column.Name))
                            .ToList()
                            .Foreach(z => z.Drop());

                        table.Indexes.Cast<Index>()
                            .Where(z => z.IndexedColumns.Cast<IndexedColumn>().Any(q => q.Name == column.Name))
                            .ToList()
                            .Foreach(z =>
                            {
                                z.Drop();
                            });
                    }

                    // ProcessDefaultConstraint(sqlMapping, newColumn);
                }
            }
        }

        private Column GetOrCreateColumn(Table table, SqlFieldMappingInfo sqlMapping)
        {
            var fieldName = sqlMapping.Name;
            var dataType = sqlMapping.SqlType;
            var isNullable = sqlMapping.IsNullable;
            var defaultConstraint = sqlMapping.DefaultConstraint;

            var column = this.GetColumn(table, sqlMapping);
            if (null == column)
            {
                column = this.CreateColumn(table, fieldName, dataType, isNullable, defaultConstraint);
            }

            return column;
        }

        private Column CreateColumn(Table table, string fieldName, DataType dataType, bool isNullable, string defaultConstraint)
        {
            var result = new Column(table, fieldName, dataType);

            defaultConstraint.MaybeString(z => result.AddDefaultConstraint().Text = z);
            table.Columns.Add(result);

            if (table.State == SqlSmoState.Creating)
            {
                table.Create();
            }
            else
            {
                result.Create();
            }

            this.Parameter.AddedColumns.Add(new Tuple<Table, Column, string>(table, result, defaultConstraint));

            return result;
        }
    }
}
