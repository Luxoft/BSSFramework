using System;
using System.Linq;

using Framework.Core;

namespace Framework.Projection;

internal static class CachedTypeExtensions
{
    private static readonly IDictionaryCache<(Type type, Type[] args), Type> MakeGenericTypeCache =
            new DictionaryCache<(Type type, Type[] args), Type>(
                                                                tuple => tuple.type.MakeGenericType(tuple.args).Pipe(tuple.type.IsCollection(), t => (Type)new CollectionOfProjectionType(t)),
                                                                new EqualityComparerImpl<(Type type, Type[] args)>((t1, t2) => t1.type == t2.type && t1.args.SequenceEqual(t2.args))).WithLock();

    public static Type CachedMakeGenericType(this Type type, params Type[] args)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));
        if (args == null) throw new ArgumentNullException(nameof(args));

        return MakeGenericTypeCache[(type, args)];
    }
}
