using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core;

public static class TypeResolverExtensions
{
    public static Type Resolve<T>(this ITypeResolver<T> typeResolver, T identity, bool failIfNotFound)
    {
        if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));

        var type = typeResolver.Resolve(identity);

        if (type == null && failIfNotFound)
        {
            throw new Exception($"Type \"{identity}\" can't be resolved.");
        }

        return type;
    }

    public static ITypeResolver<T> WithCache<T>(this ITypeResolver<T> baseTypeResolver, IEqualityComparer<T> comparer = null)
    {
        if (baseTypeResolver == null) throw new ArgumentNullException(nameof(baseTypeResolver));

        return TypeResolverHelper.Create(FuncHelper.Create((T ident) => baseTypeResolver.Resolve(ident)).WithCache(comparer),
                                         FuncHelper.Create(baseTypeResolver.GetTypes).WithCache());
    }

    public static ITypeResolver<T> WithLock<T>(this ITypeResolver<T> baseTypeResolver)
    {
        if (baseTypeResolver == null) throw new ArgumentNullException(nameof(baseTypeResolver));

        return TypeResolverHelper.Create(FuncHelper.Create((T ident) => baseTypeResolver.Resolve(ident)).WithLock(),
                                         FuncHelper.Create(baseTypeResolver.GetTypes).WithLock());
    }

    public static ITypeResolver<T> ToComposite<T>(this IEnumerable<ITypeResolver<T>> baseTypeResolver)
    {
        if (baseTypeResolver == null) throw new ArgumentNullException(nameof(baseTypeResolver));

        var typeResolvers = baseTypeResolver.ToArray();

        return TypeResolverHelper.Create((T ident) =>
                                         {
                                             var request = from typeResolver in typeResolvers

                                                           let type = typeResolver.Resolve(ident)

                                                           where type != null

                                                           select type;

                                             return request.FirstOrDefault();

                                         }, () => typeResolvers.SelectMany(z => z.GetTypes()).Distinct());
    }

    public static ITypeResolver<T> ToComposite<TSource, T>(this IEnumerable<TSource> source, Func<TSource, ITypeResolver<T>> getTypeResolver)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getTypeResolver == null) throw new ArgumentNullException(nameof(getTypeResolver));

        return source.Select(getTypeResolver).ToComposite();
    }


    public static ITypeResolver<TNewInput> OverrideInput<TInput, TNewInput>(this ITypeResolver<TInput> typeResolver, Func<TNewInput, TInput> overrideFunc)
    {
        if (typeResolver == null) throw new ArgumentNullException(nameof(typeResolver));
        if (overrideFunc == null) throw new ArgumentNullException(nameof(overrideFunc));

        return TypeResolverHelper.Create<TNewInput>(newInput => typeResolver.Resolve(overrideFunc(newInput)), () => typeResolver.GetTypes());
    }
}
