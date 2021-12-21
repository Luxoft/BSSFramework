using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> SelectMany<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return source.SelectMany(v => v);
        }

        public static void AddRange<T>(this ICollection<T> source, params T[] args)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (args == null) throw new ArgumentNullException(nameof(args));

            source.AddRange((IEnumerable<T>)args);
        }

        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> args)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (args == null) throw new ArgumentNullException(nameof(args));

            args.Foreach(source.Add);
        }

        public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in source)
            {
                var currentItem = item;
                action(currentItem);
            }
        }

        public static void Foreach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var pair in source.Select((value, index) => new { Value = value, Index = index }))
            {
                action(pair.Value, pair.Index);
            }
        }




        public static TResult Match<TSource, TResult>(this IEnumerable<TSource> source, Func<TResult> emptyFunc, Func<TSource, TResult> singleFunc, Func<TSource[], TResult> manyFunc)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (emptyFunc == null) throw new ArgumentNullException(nameof(emptyFunc));
            if (singleFunc == null) throw new ArgumentNullException(nameof(singleFunc));
            if (manyFunc == null) throw new ArgumentNullException(nameof(manyFunc));

            var cache = source.ToArray();

            switch (cache.Length)
            {
                case 0:
                    return emptyFunc();

                case 1:
                    return singleFunc(cache.Single());

                default:
                    return manyFunc(cache);
            }
        }

        public static TResult MatchE<TSource, TResult>(this IEnumerable<TSource> source, Func<TResult> emptyFunc, Func<TSource, TResult> singleFunc, Func<IEnumerable<TSource>, TResult> manyFunc)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (emptyFunc == null) throw new ArgumentNullException(nameof(emptyFunc));
            if (singleFunc == null) throw new ArgumentNullException(nameof(singleFunc));
            if (manyFunc == null) throw new ArgumentNullException(nameof(manyFunc));

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var value = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        return manyFunc(new[] { value, enumerator.Current }.Concat(enumerator.ReadToEnd()));
                    }
                    else
                    {
                        return singleFunc(value);
                    }
                }
                else
                {
                    return emptyFunc();
                }
            }
        }

        public static void Match<TSource>(this IEnumerable<TSource> source, Action emptyAction, Action<TSource> singleAction, Action<TSource[]> manyAction)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (emptyAction == null) throw new ArgumentNullException(nameof(emptyAction));
            if (singleAction == null) throw new ArgumentNullException(nameof(singleAction));
            if (manyAction == null) throw new ArgumentNullException(nameof(manyAction));

            var cache = source.ToArray();

            switch (cache.Length)
            {
                case 0:
                    emptyAction();
                    break;

                case 1:
                    singleAction(cache.Single());
                    break;

                default:
                    manyAction(cache);
                    break;
            }
        }

        public static void MatchE<TSource>(this IEnumerable<TSource> source, Action emptyAction, Action<TSource> singleAction, Action<IEnumerable<TSource>> manyAction)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (emptyAction == null) throw new ArgumentNullException(nameof(emptyAction));
            if (singleAction == null) throw new ArgumentNullException(nameof(singleAction));
            if (manyAction == null) throw new ArgumentNullException(nameof(manyAction));

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var value = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        manyAction(new[] { value }.Concat(enumerator.ReadToEnd()));
                    }
                    else
                    {
                        singleAction(value);
                    }
                }
                else
                {
                    emptyAction();
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> While<T>(this IEnumerable<T> source, Func<T, bool> splitFunc)
        {
            return source.While(splitFunc, z => z);
        }

        public static IEnumerable<IEnumerable<T>> While<T>(this IEnumerable<T> source, Func<T, bool> splitFunc, Func<T, T> processElement)
        {
            var result = new List<T>();

            foreach (var enumerable in source)
            {
                if (splitFunc(enumerable) && result.Any())
                {
                    yield return result;
                    result.Clear();
                }

                result.Add(processElement(enumerable));
            }

            if (result.Any())
            {
                yield return result;
            }
        }

        public static TResult GetByFirst<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource[], TResult> getMethod)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (getMethod == null) throw new ArgumentNullException(nameof(getMethod));

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return getMethod(enumerator.Current, enumerator.ReadToEnd().ToArray());
                }
                else
                {
                    throw new Exception("Empty source");
                }
            }
        }

        public static IEnumerable<TResult> ZipStrong<TSource, TOther, TResult>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther, TResult> resultSelector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            using (var sourceEnumerator = source.GetEnumerator())
                using (var otherEnumerator = other.GetEnumerator())
                {
                    while (true)
                    {
                        var res1 = sourceEnumerator.MoveNext();
                        var res2 = otherEnumerator.MoveNext();

                        if (res1 != res2)
                        {
                            throw new InvalidOperationException("The sequences had different lengths");
                        }

                        if (res1)
                        {
                            yield return resultSelector(sourceEnumerator.Current, otherEnumerator.Current);
                        }
                        else
                        {
                            yield break;
                        }
                    }
                }
        }

        public static IEnumerable<T> ElementsAt<T>(this IEnumerable<T> source, params int[] indices)
        {
            return source.ElementsAt((IEnumerable<int>)indices);
        }

        public static IEnumerable<T> ElementsAt<T>(this IEnumerable<T> source, IEnumerable<int> indices)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (indices == null) throw new ArgumentNullException(nameof(indices));

            var sourceCache = source.ToArray();

            return from index in indices

                   select sourceCache.ElementAt(index);
        }

        public static IEnumerable<TResult> Windowed2<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    var prev = enumerator.Current;

                    if (enumerator.MoveNext())
                    {
                        var next = enumerator.Current;

                        yield return selector(prev, next);

                        while (enumerator.MoveNext())
                        {
                            yield return selector(next, next = enumerator.Current);
                        }
                    }
                }
            }
        }

        public static void Windowed2<TSource>(this IEnumerable<TSource> source, Action<TSource, TSource> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            source.Windowed2((v1, v2) =>
            {
                selector(v1, v2);
                return default(object);
            }).ToList();
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ObservableCollection<T>(source);
        }

        public static ObservableCollection<TResult> ToObservableCollection<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToObservableCollection();
        }

        public static ObservableCollection<TResult> ToObservableCollection<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToObservableCollection();
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new Stack<T>(source);
        }

        public static Stack<TResult> ToStack<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToStack();
        }

        public static HashSet<TResult> ToHashSet<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToHashSet();
        }

        public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToArray();
        }

        public static TResult[] ToArray<TSource, TResult>([NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, int, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToArray();
        }

        public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToList();
        }

        public static ReadOnlyCollection<TSource> ToReadOnlyCollection<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new ReadOnlyCollection<TSource>(source.ToArray());
        }

        public static ReadOnlyCollection<TResult> ToReadOnlyCollection<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).ToReadOnlyCollection();
        }


        public static IEnumerable<TResult> EmptyIfNull<TResult>([CanBeNull] this IEnumerable<TResult> source)
        {
            return source ?? Enumerable.Empty<TResult>();
        }
    }
}
