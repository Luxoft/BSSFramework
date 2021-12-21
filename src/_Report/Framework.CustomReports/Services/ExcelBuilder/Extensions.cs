using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Core;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public static class Extensions
    {
        public static IList<HeaderDesign<TAnon>> ToHeaders<TAnon>(this IEnumerable<EvaluatePropertyInfo<TAnon>> source)
        {
            return source.Select(z => new HeaderDesign<TAnon>(z.Alias, z)).ToList();
        }

        public static ExcelHeaderDesign<TAnon>[] ToHeaders<TAnon>(this IEnumerable<ExcelDesignProperty<TAnon>> source)
        {
            return source.Select(z => new ExcelHeaderDesign<TAnon>(z.DesignName, z)).ToArray();
        }

        public static HeaderDesign<TAnon> ToHeader<TAnon>(this IEnumerable<EvaluatePropertyInfo<TAnon>> source, string groupHeader)
        {
            return new HeaderDesign<TAnon>(groupHeader, source.ToHeaders());
        }

        public static TResult GetValueOrDefault<TKey, TResult>(this IDictionary<TKey, TResult> source, TKey key)
        {
            TResult result;
            source.TryGetValue(key, out result);
            return result;
        }

        [Obsolete("Please use GetValueOrDefault")]
        public static TResult TryGetValue<TKey, TResult>(this IDictionary<TKey, TResult> source, TKey key)
        {
            return source.GetValueOrDefault(key);
        }

        public static int GetDeep<T>(this IEnumerable<HeaderDesign<T>> headers)
        {
            var partialResults = headers.Partial(z => z.SubHeaders.Any(), (roots, leafs) => new { roots, leafs });
            var max = partialResults.roots.Select(z => z.SubHeaders.GetDeep() + 1).Concat(new[] { 1 }).Max();
            return max;
        }

        public static IEnumerable<HeaderDesign<T>> Concat<T>(this HeaderDesign<T> source, params HeaderDesign<T>[] other)
        {
            return new[] { source }.Concat(other);
        }

        public static int GetDeep<T>(this HeaderDesign<T> header)
        {
            if (header.SubHeaders.Any())
            {
                return 1 + header.SubHeaders.GetDeep();
            }

            return 1;
        }

        public static int GetFinalCount<T>(this HeaderDesign<T> header)
        {
            if (header.SubHeaders.Any())
            {
                return header.SubHeaders.Select(GetFinalCount).Sum();
            }
            else
            {
                return 1;
            }
        }
    }
}
