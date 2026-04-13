// ReSharper disable once CheckNamespace
namespace Framework.Core.TypeResolving;

public static class TypeSourceExtensions
{
    public static ITypeResolver<TypeNameIdentity> ToDefaultTypeResolver(this ITypeSource typeSource) => TypeResolverHelper.CreateDefault(typeSource);
}
