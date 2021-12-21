using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static class EnumeratorExtensions
    {
        public static IEnumerable<T> ReadToEnd<T>(this IEnumerator<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            while (source.MoveNext())
            {
                yield return source.Current;
            }
        }

        public static T ReadSingle<T>(this IEnumerator<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.MoveNext())
            {
                return source.Current;
            }
            else
            {
                throw new Exception("enumerator finished");
            }
        }

        public static IEnumerable<T> ReadMany<T>(this IEnumerator<T> source, int count)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return Enumerable.Range(0, count).Select(_ => source.ReadSingle());
        }
    }
}
