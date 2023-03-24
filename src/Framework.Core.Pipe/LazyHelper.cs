using System;

namespace Framework.Core;

public static class LazyHelper
{
    public static Lazy<TResult> Create<TResult>(Func<TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return new Lazy<TResult>(func);
    }
}
