namespace Framework.Core;

public static class TypeSourceExtensions
{
    public static ITypeResolver<string> ToDefaultTypeResolver(this ITypeSource typeSource)
    {
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        return TypeResolverHelper.CreateDefault(typeSource);
    }
}
