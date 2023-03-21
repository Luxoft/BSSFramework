using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.CodeDom.TypeScript;

/// <summary>
/// Observable factory extensions
/// </summary>
public static class CodeTypeReferenceExtensions
{
    public static string GetTypeName(this CodeTypeReference codeTypeReference)
    {
        if (codeTypeReference.TypeArguments.Count > 0)
        {
            return codeTypeReference.TypeArguments[0].BaseType;
        }

        return codeTypeReference.BaseType;
    }

    public static string GetTypeNameWithoutNameSpace(this CodeTypeReference codeTypeReference)
    {
        return codeTypeReference.GetTypeName().GetLastKeyword();
    }

    public static string GetLastKeyword(this string data)
    {
        return data.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).LastA();
    }

    public static string ConvertToTypeScriptType(this string baseType, bool addNamespace = false, bool throwException = false )
    {
        var type = addNamespace ? $"System.{baseType}" : baseType;

        switch (type)
        {
            case "System.Byte":
            case "System.Int16":
            case "System.Int32":
            case "System.Int64":
            case "System.SByte":
            case "System.UInt16":
            case "System.UInt32":
            case "System.UInt64":
            case "System.Decimal":
            case "System.Single":
            case "System.Double":
                return "number";
            case "System.Boolean":
                return "boolean";
            case "System.Char":
            case "System.String":
                return "string";
            case "System.DateTime":
                return "Date";
            default:
                if (throwException)
                {
                    throw new NotSupportedException(nameof(baseType));
                }

                return baseType;
        }
    }
}
