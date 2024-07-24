using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator;

public class DataTypeComparer : IDataTypeComparer
{
    protected enum EqualMode
    {
        AllAspects,
        SqlDataType
    }

    protected readonly Func<DataType, DataType, bool> SqlDataTypeCompareFunc = (left, right) => left.SqlDataType == right.SqlDataType;

    public virtual bool Equals(DataType x, DataType y)
    {
        var leftEqualMode = GetEqualMode(x);
        var rightEqualMode = GetEqualMode(y);

        if (leftEqualMode != rightEqualMode)
        {
            return false;
        }

        if (EqualMode.SqlDataType == leftEqualMode)
        {
            return this.SqlDataTypeCompareFunc(x, y);
        }

        return x.SqlDataType == y.SqlDataType
               && x.Name == y.Name
               && x.Schema == y.Schema
               && x.NumericPrecision == y.NumericPrecision
               && x.NumericScale == y.NumericScale;
    }

    protected static EqualMode GetEqualMode(DataType dataType)
    {
        var sqlDataType = dataType.SqlDataType;
        switch (sqlDataType)
        {
            case SqlDataType.BigInt:
            case SqlDataType.Binary:
            case SqlDataType.Bit:
            case SqlDataType.Char:
            case SqlDataType.DateTime:
            case SqlDataType.Image:
            case SqlDataType.Int:
            case SqlDataType.Money:
            case SqlDataType.NChar:
            case SqlDataType.NText:
            case SqlDataType.NVarChar:
            case SqlDataType.NVarCharMax:
            case SqlDataType.Real:
            case SqlDataType.SmallDateTime:
            case SqlDataType.SmallInt:
            case SqlDataType.SmallMoney:
            case SqlDataType.Text:
            case SqlDataType.Timestamp:
            case SqlDataType.TinyInt:
            case SqlDataType.UniqueIdentifier:
            case SqlDataType.VarBinary:
            case SqlDataType.VarBinaryMax:
            case SqlDataType.VarChar:
            case SqlDataType.VarCharMax:
            case SqlDataType.Variant:
            case SqlDataType.Xml:
            case SqlDataType.SysName:
            case SqlDataType.Date:
            case SqlDataType.HierarchyId:
            case SqlDataType.Geometry:
            case SqlDataType.Geography:
                return EqualMode.SqlDataType;
            default:
                return EqualMode.AllAspects;
        }
    }
}
