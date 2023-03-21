using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Framework.Core;

public static class EnumHelper
{
    public static ReadOnlyCollection<TEnum> GetValues<TEnum>()
            where TEnum : struct, Enum
    {
        if (!typeof (TEnum).IsEnum)
        {
            throw new InvalidOperationException($"Type:{typeof(TEnum)} is not Enum");
        }


        return Cache<TEnum>.Values;
    }

    public static IEnumerable<Enum> GetValuesE(Type t)
    {
        if (!t.IsEnum)
        {
            throw new InvalidOperationException($"Type:{t} is not Enum");
        }
        else
        {
            return Enum.GetValues(t).OfType<Enum>();
        }
    }

    public static object[] GetValues([NotNull] Type enumType)
    {
        if (enumType == null) throw new ArgumentNullException(nameof(enumType));

        return Enum.GetValues(enumType).Cast<object>().ToArray();
    }

    public static Maybe<TEnum> MaybeParse<TEnum>(string str)
            where TEnum : struct, Enum
    {
        return Maybe.OfTryMethod(new TryMethod<string, TEnum>(Enum.TryParse)) (str);
    }

    public static TEnum Parse<TEnum>(string str)
            where TEnum : struct, Enum
    {
        return Parse<TEnum>(str, true);
    }

    public static TEnum Parse<TEnum>(string str, bool ignoreCase)
            where TEnum : struct, Enum
    {
        return (TEnum)Enum.Parse(typeof(TEnum), str, ignoreCase);
    }

    public static Maybe<TEnum> TryParse<TEnum>(string str)
            where TEnum : struct, Enum
    {
        TEnum result;

        return Enum.TryParse(str, out result) ? Maybe.Return(result)
                       : Maybe<TEnum>.Nothing;
    }

    public static FieldInfo ToFieldInfo(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return value.GetType().GetField(value.ToString());
    }

    public static string ToCSharpCode(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return value.GetType().FullName + "." + value;
    }


    private static class Cache<TEnum>
    {
        public static readonly ReadOnlyCollection<TEnum> Values = GetValues().ToReadOnlyCollection();

        private static IEnumerable<TEnum> GetValues()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new InvalidOperationException($"Type:{typeof(TEnum)} is not Enum");
            }

            return typeof(TEnum).GetFields().Where(z => z.IsLiteral).Select(z => (TEnum)z.GetValue(null));
        }
    }
}
