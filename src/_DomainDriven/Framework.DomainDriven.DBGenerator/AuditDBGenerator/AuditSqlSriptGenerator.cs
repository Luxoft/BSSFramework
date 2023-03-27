using System.Data;
using System.Text;
using Framework.Core;

using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Mapping;
using NHibernate.Util;

namespace Framework.DomainDriven.NHibernate;

/// <summary>
/// Audit DB Sql Sript generator
/// </summary>
internal class AuditSqlSriptGenerator
{
    public string[] GenerateCreateScripts(AuditTableGenerateContext context)
    {
        var table = context.Table;
        var dialect = context.Dialect;
        var defaultCatalog = context.DefaultCatalog;
        var defaultSchema = context.DefaultSchema;
        var mapping = context.Mapping;

        var buf =
                new StringBuilder(context.Table.HasPrimaryKey ? context.Dialect.CreateTableString : context.Dialect.CreateMultisetTableString)
                        .Append(' ')
                        .Append(context.GetQualifiedTableName()).Append(" (");

        bool identityColumn = table.IdentifierValue != null && table.IdentifierValue.IsIdentityColumn(dialect);

        // try to find out the name of the pk to create it as identity if the
        // identitygenerator is used
        string pkName = null;
        if (table.HasPrimaryKey && identityColumn)
        {
            pkName = table.PrimaryKey.ColumnIterator.Single().GetQuotedName(dialect);
        }

        var commaNeeded = false;

        var nextColumns = GetAuditedColumns(context, table);

        foreach (var col in nextColumns)
        {
            if (commaNeeded)
            {
                buf.Append(StringHelper.CommaSpace);
            }
            commaNeeded = true;

            buf.Append(col.GetQuotedName(dialect)).Append(' ');

            if (identityColumn && col.GetQuotedName(dialect).Equals(pkName))
            {
                // to support dialects that have their own identity data type
                if (dialect.HasDataTypeInIdentityColumn)
                {
                    buf.Append(col.GetSqlType(dialect, mapping));
                }

                buf.Append(' ').Append(dialect.GetIdentityColumnString(col.GetSqlTypeCode(mapping).DbType));
            }
            else
            {

                var sqlType = this.GetColumnSqlType(context, col);

                buf.Append(sqlType);

                if (!string.IsNullOrEmpty(col.DefaultValue))
                {
                    buf.Append(" default ").Append(col.DefaultValue).Append(" ");
                }

                if (col.IsNullable)
                {
                    buf.Append(dialect.NullColumnString);
                }
                else
                {
                    buf.Append(" not null");
                }
            }

            if (col.IsUnique)
            {
                if (dialect.SupportsUnique)
                {
                    buf.Append(" unique");
                }
                else
                {
                    var uk = table.GetUniqueKey(col.GetQuotedName(dialect) + "_");
                    uk.AddColumn(col);
                }
            }

            if (col.HasCheckConstraint && dialect.SupportsColumnCheck)
            {
                buf.Append(" check( ").Append(col.CheckConstraint).Append(") ");
            }

            if (string.IsNullOrEmpty(col.Comment) == false)
            {
                buf.Append(dialect.GetColumnComment(col.Comment));
            }
        }

        if (table.HasPrimaryKey && (dialect.GenerateTablePrimaryKeyConstraintForIdentityColumn || !identityColumn))
        {
            buf.Append(StringHelper.CommaSpace).Append(table.PrimaryKey.SqlConstraintString(dialect, defaultSchema));
        }

        foreach (UniqueKey uk in table.UniqueKeyIterator)
        {
            buf.Append(',').Append(uk.SqlConstraintString(dialect));
        }

        if (dialect.SupportsTableCheck)
        {
            foreach (string checkConstraint in table.CheckConstraintsIterator)
            {
                buf.Append(", check (").Append(checkConstraint).Append(") ");
            }
        }

        if (!dialect.SupportsForeignKeyConstraintInAlterTable)
        {
            foreach (var foreignKey in table.ForeignKeyIterator)
            {
                if (foreignKey.HasPhysicalConstraint)
                {
                    buf.Append(",").Append(foreignKey.SqlConstraintString(dialect, foreignKey.Name, defaultCatalog, defaultSchema));
                }
            }
        }

        buf.Append(StringHelper.ClosedParen);

        if (string.IsNullOrEmpty(table.Comment) == false)
        {
            buf.Append(dialect.GetTableComment(table.Comment));
        }

        buf.Append(dialect.TableTypeString);

        return new[] { buf.ToString() };
    }

    private static IEnumerable<Column> GetAuditedColumns(AuditTableGenerateContext context, Table table)
    {
        var columnIterator = table.ColumnIterator;

        var groupedColumns =
                columnIterator.GroupBy(
                                       z => z.Name.ToLower().Replace("[", string.Empty).Replace("]", string.Empty).Replace("id", string.Empty).Replace("_mod", string.Empty).ToLower())
                              .ToList();

        if (context.Original == null)
        {
            return table.ColumnIterator;
        }

        var pairs =
                groupedColumns.Join(
                                    context.Original.PropertyIterator,
                                    z => z.Key,
                                    z => z.Name.ToLower(),
                                    (groupedColumn, property) => new { groupedColumn, property }).ToList();

        var filterColumns =
                pairs.Where(
                            z =>
                                    context.AuditAttributeService.GetAttributeFor(context.Original.MappedClass, z.property)
                                    == RelationTargetAuditMode.Audited).SelectMany(z => z.groupedColumn).ToList();

        var additionalColumns = table.ColumnIterator.Except(pairs.SelectMany(z => z.groupedColumn)).ToList();

        var result = filterColumns.Concat(additionalColumns).ToList();
        return result;
    }

    private string GetColumnSqlType(AuditTableGenerateContext context, Column column)
    {
        var mapping = context.Mapping;
        var dialect = context.Dialect;


        var sqlType = column.GetSqlType(dialect, mapping);

        // nhib feature: for generate 'NVARCHAR(MAX)' length must be initialize int.Max/2
        if (column.Length == int.MaxValue && column.GetSqlTypeCode(mapping).DbType == DbType.String)
        {
            sqlType = "NVARCHAR(MAX)";
        }

        var clearColumnName = column.Name.Skip("[").SkipLast("]");

        if (sqlType.ToLower().Contains("decimal"))
        {
            return sqlType;
        }

        context
                .OriginalTableMetadata
                .Maybe(z => z.GetColumnMetadata(clearColumnName)
                             .Maybe(q => sqlType = (q.TypeName.Contains("image") || q.TypeName.Contains("text") || q.ColumnSize == 0) ? q.TypeName :
                                                           $"{q.TypeName}({(q.ColumnSize == -1 ? "max" : q.ColumnSize.ToString())})"));
        return sqlType;
    }

    public string[] GenerateUpdateScripts(AuditTableGenerateContext context)
    {
        var table = context.Table;
        var dialect = context.Dialect;
        var tableInfo = context.TableInfo;
        var rootTemplate =
                new StringBuilder("alter table ").Append(context.GetQualifiedTableName()).Append(' ');


        var results = new List<string>(table.ColumnSpan);

        var nextColumns = GetAuditedColumns(context, table);

        foreach (var column in nextColumns)
        {
            var columnName = column.Name;

            var clearColumnName = columnName.Skip("[").SkipLast("]");

            var actualColumnSqlType = this.GetColumnSqlType(context, column);

            var addOrUpdate = dialect.AddColumnString;

            var columnInfoBase = tableInfo.GetColumnMetadata(clearColumnName);

            var columnInfo = columnInfoBase as IColumnMetadataExtended;

            if (columnInfoBase != null && columnInfo == null)
            {
                throw new ArgumentException($"Can't cast to {nameof(IColumnMetadataExtended)}");
            }

            if (columnInfo != null)
            {
                var expectedColumnTypes = new[]
                                          {
                                                  columnInfo.TypeName.ToLower(),
                                                  $"{columnInfo.TypeName.ToLower()}({(columnInfo.ColumnSize == -1 ? "max" : columnInfo.ColumnSize.ToString())})",
                                                  $"{columnInfo.TypeName.ToLower()}({columnInfo.NumericalPrecision},{columnInfo.NumericalScale})"
                                          };

                var lowActualColumnSqlType = new string(actualColumnSqlType.Where(z => z != ' ').ToArray()).ToLower();

                if (expectedColumnTypes.Contains(lowActualColumnSqlType))
                {
                    continue;
                }

                addOrUpdate = "alter column";
            }

            // the column doesnt exist at all.
            var alter = new StringBuilder(
                                          rootTemplate.ToString())
                        .Append(' ')
                        .Append(addOrUpdate)
                        .Append(' ')
                        .Append(column.GetQuotedName(dialect))
                        .Append(' ')
                        .Append(actualColumnSqlType);

            var defaultValue = column.DefaultValue;
            if (!string.IsNullOrEmpty(defaultValue))
            {
                alter.Append(" default ").Append(defaultValue);

                alter.Append(column.IsNullable ? dialect.NullColumnString : " not null");
            }

            var useUniqueConstraint = column.Unique && dialect.SupportsUnique
                                                    && (!column.IsNullable || dialect.SupportsNotNullUnique);
            if (useUniqueConstraint)
            {
                alter.Append(" unique");
            }

            if (column.HasCheckConstraint && dialect.SupportsColumnCheck)
            {
                alter.Append(" check(").Append(column.CheckConstraint).Append(") ");
            }

            var columnComment = column.Comment;
            if (columnComment != null)
            {
                alter.Append(dialect.GetColumnComment(columnComment));
            }

            results.Add(alter.ToString());
        }

        return results.ToArray();
    }
}
