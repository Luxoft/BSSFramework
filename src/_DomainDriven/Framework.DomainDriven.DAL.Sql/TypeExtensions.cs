using Framework.Core;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DAL.Sql;

public static class TypeExtensions
{
    public static DataType ToDataType(this Type type, IEnumerable<Attribute> propertyAttributies)
    {
        if (type.IsNullable())
        {
            return type.GetGenericArguments()[0].ToDataType(propertyAttributies);
        }
        if (type.IsArray && type.GetElementType() == typeof(byte) && propertyAttributies.All(z=>z.GetType() != typeof(VersionAttribute)))
        {
            return DataType.Image;
        }
        if (type.IsArray && type.GetElementType() == typeof(byte) && propertyAttributies.Any(z => z.GetType() == typeof(VersionAttribute)))
        {
            return DataType.Timestamp;
        }

        if (type.IsEnum)
        {
            return DataType.Int;
        }
        if (type == typeof(long))
        {
            return DataType.BigInt;
        }
        if (type == typeof(bool))
        {
            return DataType.Bit;
        }
        if (type == typeof(string))
        {
            var length = GetValueByAttribute<int, MaxLengthAttribute>(propertyAttributies, z => z.Value, 255);

            var notUseUnitCode = propertyAttributies.Any(attr => attr is UseAsciiAttribute);

            var useUnicode = !notUseUnitCode;

            if (length != int.MaxValue && length >= (useUnicode ? 4000 : 8000))
            {
                return useUnicode ? DataType.NText : DataType.Text;
            }
            return useUnicode ? length == int.MaxValue ? DataType.NVarCharMax : DataType.NVarChar(length) : length == int.MaxValue ? DataType.VarCharMax : DataType.VarChar(length);
        }
        if (type == typeof(DateTime))
        {
            return DataType.DateTime;
        }
        if (type == typeof(decimal))
        {
            var attribute = propertyAttributies.OfType<LengthAndPrecisionAttribute>().FirstOrDefault() ?? LengthAndPrecisionAttribute.Default;

            return DataType.Decimal(attribute.Precision, attribute.Length);
        }
        if (type == typeof(int))
        {
            return DataType.Int;
        }
        if (type == typeof(float))
        {
            return DataType.Float;
        }
        if (type == typeof(Guid))
        {
            return DataType.UniqueIdentifier;
        }

        if (type == typeof(double))
        {
            return DataType.Real;
        }

        if (type == typeof(byte))
        {
            return DataType.TinyInt;
        }



        throw new System.ArgumentException($"Type:{type.Name} have not CRLType analog");

        //Case Smo.SqlDataType.Real, Smo.SqlDataType.Numeric, Smo.SqlDataType.Float
        //    If col.DataType.Equals(Smo.DataType.Float) Then
        //        Dim i As Int32 = 1
        //    End If
        //    typ = GetType(System.Double)
        //Case Smo.SqlDataType.Timestamp, Smo.SqlDataType.Binary
        //    typ = GetType(System.Byte())
        //Case Smo.SqlDataType.TinyInt, Smo.SqlDataType.SmallInt
        //    typ = GetType(System.Int16)
        //Case Smo.SqlDataType.UniqueIdentifier
        //    typ = GetType(System.Guid)
        //Case Smo.SqlDataType.UserDefinedDataType, Smo.SqlDataType.UserDefinedType, Smo.SqlDataType.Variant, Smo.SqlDataType.Image
        //    typ = GetType(Object)
        //Case Else
        //    typ = GetType(System.String)
    }
    private static TValue GetValueByAttribute<TValue, TAttribute>(IEnumerable<Attribute> attributes, Func<TAttribute, TValue> getValueFunc, TValue defaultValue)
    {
        var attribute = attributes.OfType<TAttribute>()
                                  .Concat(attributes.OfType<IConvertible<TAttribute>>().Select(z=>z.Convert())).FirstOrDefault();
        return null != attribute ? getValueFunc(attribute) : defaultValue;
    }

}
