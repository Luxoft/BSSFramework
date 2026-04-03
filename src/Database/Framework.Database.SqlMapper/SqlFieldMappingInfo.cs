using Framework.Database.Metadata;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.SqlMapper;

public class SqlFieldMappingInfo(
    DataType sqlType,
    string name,
    bool isNullable,
    bool isPrimaryKey,
    FieldMetadata source,
    string defaultConstraint,
    bool isUniqueKey = false)
{
    public string Name => name;

    public DataType SqlType => sqlType;

    public bool IsNullable => isNullable;

    public bool IsPrimaryKey => isPrimaryKey;

    public FieldMetadata Source => source;

    public bool IsUniqueKey => isUniqueKey;

    public string DefaultConstraint => defaultConstraint;
}
