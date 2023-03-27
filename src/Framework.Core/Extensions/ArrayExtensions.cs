namespace Framework.Core;

public static class ArrayExtensions
{
    public static bool AnyA(this Array array)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));

        return array.Length != 0;
    }

    public static T LastA<T>(this T[] array)
    {
        if (array == null) throw new ArgumentNullException(nameof(array));

        return array.AnyA() ? array[array.Length - 1] : array.Last();
    }
}
