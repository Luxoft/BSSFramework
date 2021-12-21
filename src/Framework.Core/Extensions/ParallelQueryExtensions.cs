using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static class ParallelQueryExtensions
    {
        public static List<TResult> ToList<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));


            return source.Select(selector).ToList();
        }

        public static TResult[] ToArray<TSource, TResult>(this ParallelQuery<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));


            return source.Select(selector).ToArray();
        }

        public static ParallelQuery<T> Distinct<T>(this ParallelQuery<T> source, Func<T, T, bool> equalsFunc, Func<T, int> getHashFunc = null)
        {
            return source.Distinct(new EqualityComparerImpl<T>(equalsFunc, getHashFunc));
        }
    }
}