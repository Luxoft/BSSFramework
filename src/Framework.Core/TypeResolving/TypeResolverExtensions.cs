namespace Framework.Core.TypeResolving;

public static class TypeResolverExtensions
{
    public static ITypeResolver<T> ToComposite<T>(this IEnumerable<ITypeResolver<T>> baseTypeResolver)
        where T : notnull
    {
        var typeResolvers = baseTypeResolver.ToArray();

        return TypeResolverHelper.Create(
            (T ident) =>
            {
                var request = from typeResolver in typeResolvers

                              let type = typeResolver.TryResolve(ident)

                              where type != null

                              select type;

                return request.FirstOrDefault();

            },
            typeResolvers.SelectMany(z => z.Types));
    }

    public static ITypeResolver<T> ToComposite<TSource, T>(this IEnumerable<TSource> source, Func<TSource, ITypeResolver<T>> getTypeResolver)
        where T : notnull =>
        source.Select(getTypeResolver).ToComposite();

    public static ITypeResolver<TNewInput> OverrideInput<TInput, TNewInput>(this ITypeResolver<TInput> typeResolver, Func<TNewInput, TInput> overrideFunc)
        where TNewInput : notnull =>
        TypeResolverHelper.Create<TNewInput>(newInput => typeResolver.TryResolve(overrideFunc(newInput)), typeResolver.Types);
}
