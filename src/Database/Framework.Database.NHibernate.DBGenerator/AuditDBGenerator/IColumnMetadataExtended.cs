using NHibernate.Dialect.Schema;

namespace Framework.Database.NHibernate.DBGenerator.AuditDBGenerator;

internal interface IColumnMetadataExtended : IColumnMetadata
{
    int NumericalScale { get; }
}
