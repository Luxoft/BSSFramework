using System;
using System.Collections.Generic;

namespace Framework.Core.Serialization;

public static class FormatterExtensions
{
    public static IFormatter<TValue, TResult> WithCache<TValue, TResult>(IFormatter<TValue, TResult> formatter, IEqualityComparer<TValue> equalityComparer = null)
    {
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return new Formatter<TValue, TResult>(FuncHelper.Create((TValue value) => formatter.Format(value)).WithCache(equalityComparer));
    }

    public static IFormatter<TValue, TResult> WithLock<TValue, TResult>(IFormatter<TValue, TResult> formatter)
    {
        if (formatter == null) throw new ArgumentNullException(nameof(formatter));

        return new Formatter<TValue, TResult>(FuncHelper.Create((TValue value) => formatter.Format(value)).WithLock());
    }
}
