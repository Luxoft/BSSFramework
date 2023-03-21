using Framework.DomainDriven.Metadata;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DAL.Sql;

public class SqlFieldMappingInfo
{
    private readonly DataType _sqlType;
    private readonly string _name;
    private readonly bool _isNullable;
    private readonly bool _isPrimaryKey;
    private readonly bool _isUniqueKey;

    private readonly FieldMetadata _source;
    private readonly string _defaultConstraint;

    public SqlFieldMappingInfo(DataType sqlType,
                               string name,
                               bool isNullable,
                               bool isPrimaryKey,
                               FieldMetadata source,
                               string defaultConstraint,
                               bool isUniqueKey = false)
    {
        this._sqlType = sqlType;
        this._name = name;
        this._isNullable = isNullable;
        this._isPrimaryKey = isPrimaryKey;
        this._source = source;
        this._defaultConstraint = defaultConstraint;
        this._isUniqueKey = isUniqueKey;
    }
    public string Name
    {
        get { return this._name; }
    }
    public DataType SqlType
    {
        get { return this._sqlType; }
    }
    public bool IsNullable
    {
        get
        {
            return this._isNullable;
        }
    }
    public bool IsPrimaryKey
    {
        get { return this._isPrimaryKey; }
    }
    public FieldMetadata Source
    {
        get { return this._source; }
    }

    public bool IsUniqueKey
    {
        get { return this._isUniqueKey; }
    }

    public string DefaultConstraint
    {
        get { return this._defaultConstraint; }
    }
}
