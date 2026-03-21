namespace Framework.Validation;

public static class EnumExtensions
{
    public static bool HasFlag(this int source, int target) => (source & target) == target;

    public static bool IsIntersected(this int source, int target) => (source & target) != 0;
}
