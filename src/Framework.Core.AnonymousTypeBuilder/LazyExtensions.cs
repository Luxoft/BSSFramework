using System;
using System.Collections.Generic;

namespace Framework.Core;

public static class LazyExtensions
{
    public static IEnumerable<T> Unwrap<T>(this Lazy<IEnumerable<T>> lazyEnumerable)
    {
        if (lazyEnumerable == null) throw new ArgumentNullException(nameof(lazyEnumerable));

        return LazyInterfaceImplementHelper.CreateProxy(() => lazyEnumerable.Value);
    }

    //public static T GetValueOr<T>(this Lazy<T> lazyValue, Func<T> orFunc)
    //{
    //    return lazyValue.IsValueCreated ? lazyValue.Value : orFunc();
    //}
}
