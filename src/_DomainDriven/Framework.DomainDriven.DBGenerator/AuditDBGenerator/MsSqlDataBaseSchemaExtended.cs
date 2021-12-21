using System;
using System.Data;
using System.Data.Common;

using NHibernate.Dialect.Schema;

namespace Framework.DomainDriven.NHibernate
{
    public class MsSqlDataBaseSchemaExtended : AbstractDataBaseSchema
    {
        public MsSqlDataBaseSchemaExtended(DbConnection connection) : base(connection) { }

        public override ITableMetadata GetTableMetadata(DataRow rs, bool extras)
        {
            return new MsSqlTableMetadataExtended(rs, this, extras);
        }

        public class MsSqlTableMetadataExtended : AbstractTableMetadata
        {
            public MsSqlTableMetadataExtended(DataRow rs, IDataBaseSchema meta, bool extras) : base(rs, meta, extras) { }

            protected override void ParseTableInfo(DataRow rs)
            {
                // Clearly, we cannot use the same names when connected via ODBC...
                this.Catalog = SchemaHelper.GetString(rs, "TABLE_CATALOG", "TABLE_CAT");
                this.Schema = SchemaHelper.GetString(rs, "TABLE_SCHEMA", "TABLE_SCHEM");
                if (string.IsNullOrEmpty(this.Catalog)) this.Catalog = null;
                if (string.IsNullOrEmpty(this.Schema)) this.Schema = null;
                this.Name = Convert.ToString(rs["TABLE_NAME"]);
            }

            protected override string GetConstraintName(DataRow rs)
            {
                return Convert.ToString(rs["CONSTRAINT_NAME"]);
            }

            protected override string GetColumnName(DataRow rs)
            {
                return Convert.ToString(rs["COLUMN_NAME"]);
            }

            protected override string GetIndexName(DataRow rs)
            {
                return Convert.ToString(rs["INDEX_NAME"]);
            }

            protected override IColumnMetadata GetColumnMetadata(DataRow rs)
            {
                return new MsSqlColumnMetadataExtended(rs);
            }

            protected override IForeignKeyMetadata GetForeignKeyMetadata(DataRow rs)
            {
                return new MsSqlForeignKeyMetadata(rs);
            }

            protected override IIndexMetadata GetIndexMetadata(DataRow rs)
            {
                return new MsSqlIndexMetadata(rs);
            }
        }

        public class MsSqlColumnMetadataExtended : AbstractColumnMetaData, IColumnMetadataExtended
        {
            public MsSqlColumnMetadataExtended(DataRow rs) : base(rs)
            {
                this.Name = Convert.ToString(rs["COLUMN_NAME"]);

                // Clearly, we cannot use the same names when connected via ODBC...
                this.SetColumnSize(SchemaHelper.GetValue(rs, "CHARACTER_MAXIMUM_LENGTH", "COLUMN_SIZE"));
                this.SetNumericalPrecision(SchemaHelper.GetValue(rs, "NUMERIC_PRECISION", "COLUMN_SIZE"));
                this.SetNumericalScale(SchemaHelper.GetValue(rs, "NUMERIC_SCALE"));

                this.Nullable = Convert.ToString(rs["IS_NULLABLE"]);

                // For the type name, DATA_TYPE is numeric when using ODBC, so use the
                // string-valued ODBC-only TYPE_NAME as first alternative.
                this.TypeName = SchemaHelper.GetString(rs, "TYPE_NAME", "DATA_TYPE");
            }

            protected void SetNumericalScale(object numericalScaleValue)
            {
                if (numericalScaleValue != DBNull.Value)
                {
                    this.NumericalScale = Convert.ToInt32(numericalScaleValue);
                }
            }

            public int NumericalScale { get; private set; }
        }

    }
}
