namespace Framework.Core;

public static class DelegateExtensions
{
    public static System.Reflection.MethodInfo CreateGenericMethod(this Delegate source, params Type[] types)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (types == null) throw new ArgumentNullException(nameof(types));

        return source.Method.GetGenericMethodDefinition().MakeGenericMethod(types);
    }
}
