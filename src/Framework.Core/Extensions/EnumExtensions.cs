using System;

namespace Framework.Core;

public static class EnumExtensions
{
    public static bool IsDefaultEnumValue(this Enum @enum)
    {
        if (@enum == null) throw new ArgumentNullException(nameof(@enum));

        return Enum.ToObject(@enum.GetType(), 0).Equals(@enum);
    }
}
