namespace Framework.Core;

public static class EnumExtensions
{
    public static string ToCSharpCode(this Enum value)
    {
        if (value == null) throw new ArgumentNullException(nameof(value));

        return value.GetType().FullName + "." + value;
    }
}
