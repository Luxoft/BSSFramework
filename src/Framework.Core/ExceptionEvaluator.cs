using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static class ExceptionEvaluator
    {
        [Obsolete("v10 This method will be protected in future")]
        public static TResult Evaluate<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, Func<IEnumerable<Exception>, Exception> getAggregateException)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (getAggregateException == null) throw new ArgumentNullException(nameof(getAggregateException));

            using (var enumerator = source.GetEnumerator())
            {
                return enumerator.Evaluate(selector, new Exception[0], getAggregateException);
            }
        }

        private static TResult Evaluate<TSource, TResult>(this IEnumerator<TSource> source, Func<TSource, TResult> selector, IEnumerable<Exception> exceptions, Func<IEnumerable<Exception>, Exception> getAggregateException)
        {
            if (source.MoveNext())
            {
                try
                {
                    return selector(source.Current);
                }
                catch (Exception ex)
                {
                    return source.Evaluate(selector, exceptions.Concat(new[] { ex }), getAggregateException);
                }
            }
            else
            {
                throw getAggregateException(exceptions);
            }
        }
    }
}
