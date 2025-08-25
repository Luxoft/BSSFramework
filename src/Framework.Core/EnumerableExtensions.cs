using System.Collections.ObjectModel;

using CommonFramework;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class EnumerableExtensions
{
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

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return !source.Any();
    }

    public static ReadOnlyCollection<TResult> ToReadOnlyCollection<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).ToReadOnlyCollection();
    }

    public static TResult[] ToArray<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).ToArray();
    }

    public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource element, IEqualityComparer<TSource>? comparer = null)
    {
        var index = 0;

        var usedComparer = comparer ?? EqualityComparer<TSource>.Default;

        foreach (var value in source)
        {
            if (usedComparer.Equals(element, value))
            {
                return index;
            }
            else
            {
                index++;
            }
        }

        return -1;
    }

    public static IEnumerable<TState> Scan<TSource, TState>(this IEnumerable<TSource> source, TState state, Func<TState, TSource, TState> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        yield return state;

        foreach (var item in source)
        {
            yield return state = selector(state, item);
        }
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

    public static TResult Partial<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<List<TSource>, List<TSource>, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        var l1 = new List<TSource>();
        var l2 = new List<TSource>();

        foreach (var item in source)
        {
            (predicate(item) ? l1 : l2).Add(item);
        }

        return selector(l1, l2);
    }

    public static ReadOnlyCollection<TSource> ToReadOnlyCollection<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new ReadOnlyCollection<TSource>(source.ToArray());
    }

    public static List<TResult> ToList<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).ToList();
    }

    public static MergeResult<T, T> GetMergeResult<T>(this IEnumerable<T> source, IEnumerable<T> target, IEqualityComparer<T>? comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));

        return source.GetMergeResult(target, v => v, v => v, comparer);
    }

    public static MergeResult<TSource, TTarget> GetMergeResult<TSource, TTarget, TKey>(
        this IEnumerable<TSource> source,
        IEnumerable<TTarget> target,
        Func<TSource, TKey> sourceKeySelector,
        Func<TTarget, TKey> targetKeySelector,
        IEqualityComparer<TKey>? comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));
        if (sourceKeySelector == null) throw new ArgumentNullException(nameof(sourceKeySelector));
        if (targetKeySelector == null) throw new ArgumentNullException(nameof(targetKeySelector));

        var targetMap = target.ToDictionary(targetKeySelector, z => z, comparer ?? EqualityComparer<TKey>.Default);

        var removingItems = new List<TSource>();

        var combineItems = new List<ValueTuple<TSource, TTarget>>();

        foreach (var sourceItem in source)
        {
            var sourceKey = sourceKeySelector(sourceItem);

            if (targetMap.TryGetValue(sourceKey, out var targetItem))
            {
                combineItems.Add(ValueTuple.Create(sourceItem, targetItem));
                targetMap.Remove(sourceKey);
            }
            else
            {
                removingItems.Add(sourceItem);
            }
        }

        var addingItems = targetMap.Values.ToList();

        return new MergeResult<TSource, TTarget>(addingItems, combineItems, removingItems);
    }

    public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, T expectedElement, bool raiseIfNotEquals = false)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Skip(expectedElement, EqualityComparer<T>.Default, raiseIfNotEquals);
    }

    public static IEnumerable<T> Skip<T>(this IEnumerable<T> source, T expectedElement, IEqualityComparer<T> equalityComparer, bool raiseIfNotEquals = false)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

        using (var enumerator = source.GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                if (!equalityComparer.Equals(enumerator.Current, expectedElement))
                {
                    if (raiseIfNotEquals)
                    {
                        throw new ArgumentException("Unexpected first element", nameof(source));
                    }

                    yield return enumerator.Current;
                }

                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
            else
            {
                if (raiseIfNotEquals)
                {
                    throw new ArgumentException("EmptyCollection", nameof(source));
                }
            }
        }
    }

    public static IEnumerable<T> CollectMaybe<T>(this IEnumerable<Maybe<T>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return from item in source
               let just = item as Just<T>
               where just != null
               select just.Value;
    }

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> manyExceptionHandler)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        if (manyExceptionHandler == null)
        {
            throw new ArgumentNullException(nameof(manyExceptionHandler));
        }

        return source.Where(predicate).SingleOrDefault(manyExceptionHandler);
    }

    public static IEnumerable<T> MaybeYield<T>(this T source)
        where T : class
    {
        if (source != null)
        {
            yield return source;
        }
    }
}
