using System;
using System.Collections.Generic;

namespace Framework.Core
{
    public static class CodeTypeExtensions
    {
        private static readonly Dictionary<TypeCode, string> MapDict = new Dictionary<TypeCode, string>
        {
            { TypeCode.Boolean, "boolean" },
            { TypeCode.Byte, "unsignedByte" },
            { TypeCode.Char, "char" },
            { TypeCode.DateTime, "dateTime" },
            { TypeCode.Decimal, "decimal" },
            { TypeCode.Double, "double" },
            { TypeCode.Int16, "short" },
            { TypeCode.Int32, "int" },
            { TypeCode.Int64, "long" },
            { TypeCode.SByte, "byte" },
            { TypeCode.Single, "float" },
            { TypeCode.String, "string" },
            { TypeCode.UInt16, "unsignedShort" },
            { TypeCode.UInt32, "unsignedInt" },
            { TypeCode.UInt64, "unsignedLong" },
        };

        private static readonly Dictionary<string, TypeCode> ReverseMapDict = MapDict.Reverse();


        public static Maybe<string> TryGetSerializeString (this TypeCode typeCode)
        {
            return MapDict.GetMaybeValue(typeCode);
        }

        public static Maybe<TypeCode> TryGetSerializeTypeCode(this string str)
        {
            return ReverseMapDict.GetMaybeValue(str);
        }
    }
}