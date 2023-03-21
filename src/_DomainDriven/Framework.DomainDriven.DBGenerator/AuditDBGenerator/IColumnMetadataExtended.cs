using NHibernate.Dialect.Schema;

namespace Framework.DomainDriven.NHibernate;

internal interface IColumnMetadataExtended : IColumnMetadata
{
    int NumericalScale { get; }
}
