namespace Framework.Core.TypeResolving.TypeSource;

public static class TypeSourceExtensions
{
    public static ITypeResolver<string> ToDefaultTypeResolver(this ITypeSource typeSource) => TypeResolverHelper.CreateDefault(typeSource);
}
