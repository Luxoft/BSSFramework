using System.Collections.ObjectModel;

using CommonFramework;
using CommonFramework.Maybe;

namespace Framework.Core;

public static class CoreEnumerableExtensions
{

    public static void Foreach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (action == null) throw new ArgumentNullException(nameof(action));

        foreach (var pair in source.Select((value, index) => new { Value = value, Index = index }))
        {
            action(pair.Value, pair.Index);
        }
    }

    public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
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

    public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (manyExceptionHandler == null)
        {
            throw new ArgumentNullException(nameof(manyExceptionHandler));
        }

        var items = source.ToList();

        if (items.Count > 1)
        {
            throw manyExceptionHandler(items.ToReadOnlyCollection());
        }

        return items.SingleOrDefault();
    }

    public static IGrouping<TKey, TElement> ToGroup<TKey, TElement>(this TKey key, IEnumerable<TElement> values)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));

        return key.ToKeyValuePair(values).ToGroup();
    }

    public static IGrouping<TKey, TElement> ToGroup<TKey, TElement>(this KeyValuePair<TKey, IEnumerable<TElement>> pair)
    {
        return new PairGrouping<TKey, TElement>(pair.Value.ToList(), pair.Key);
    }

    public static bool HasAttribute<T>(this IEnumerable<Attribute> source)
        where T : Attribute
    {
        return source.OfType<T>().Any();
    }

    public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> source, Func<IEnumerable<T>> func)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var sourceEnumerator = source.GetEnumerator())
        {
            if (sourceEnumerator.MoveNext())
            {
                do
                {
                    yield return sourceEnumerator.Current;
                } while (sourceEnumerator.MoveNext());
            }
            else
            {
                using (var otherEnumerator = func().GetEnumerator())
                {
                    while (otherEnumerator.MoveNext())
                    {
                        yield return otherEnumerator.Current;
                    }
                }
            }
        }
    }

    public static int GetIndex<T>(this IEnumerable<T> source, T value)
    {
        var currentIndex = 0;

        foreach (var item in source)
        {
            if (EqualityComparer<T>.Default.Equals(item, value))
            {
                return currentIndex;
            }

            ++currentIndex;
        }

        return -1;
    }

    public static T FirstOr<T>(this IEnumerable<T> source, Func<T> func)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var enumerator = source.GetEnumerator())
        {
            return enumerator.MoveNext() ? enumerator.Current : func();
        }
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollectionI<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToList();
    }

    public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
        where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(keySelector, elementSelector));
    }

    public static ReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToReadOnlyDictionary(keySelector, v => v);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionaryI<TValue, TKey>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
        where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        return source.ToReadOnlyDictionary(keySelector);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionaryI<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        where TKey : notnull
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static void Override<T>(this ICollection<T> source, IEnumerable<T> newItems)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (newItems == null) throw new ArgumentNullException(nameof(newItems));

        source.Clear();

        newItems.Foreach(source.Add);
    }

    public static TSource First<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (emptyExceptionHandler == null)
        {
            throw new ArgumentNullException(nameof(emptyExceptionHandler));
        }

        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                throw emptyExceptionHandler();
            }

            return enumerator.Current;
        }
    }


    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> equalsFunc, Func<T, int>? getHashFunc = null)
    {
        return source.Distinct(new EqualityComparerImpl<T>(equalsFunc, getHashFunc));
    }

    public static void Merge<T, S, TKey>(
        this IEnumerable<T> source,
        IEnumerable<S> target,
        Func<S, TKey> getSKey,
        Func<T, TKey> getTKey,
        Func<S, T> createAndMapFunc,
        Action<IEnumerable<T>> removeAction,
        Func<TKey, bool>? isDefaultKey = null)
    {
        var isDefaultKeyF = isDefaultKey ?? (v => v.IsDefault());

        var targetMap = target.ToDictionary(s => isDefaultKeyF(getSKey(s)) ? new object() : getSKey(s));

        var removed = new List<T>();

        foreach (var sourceItem in source.ToList())
        {
            var key = getTKey(sourceItem);
            if (targetMap.TryGetValue(key, out var targetItem))
            {
                createAndMapFunc(targetItem);
                targetMap.Remove(key);
            }
            else
            {
                removed.Add(sourceItem);
            }
        }

        removeAction(removed);

        foreach (var sourceItem in targetMap.Values)
        {
            createAndMapFunc(sourceItem);
        }
    }

    public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int size)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (size < 1) throw new ArgumentException("count");

        using (var enumerator = source.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.InnerSplit(size).ToList();
            }
        }
    }

    private static IEnumerable<TSource> InnerSplit<TSource>(this IEnumerator<TSource> source, int size)
    {
        do
        {
            yield return source.Current;
        } while ((--size) != 0 && source.MoveNext());
    }

    public static Stack<T> ToStack<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new Stack<T>(source);
    }

    public static IEnumerable<T> Distinct<T, TProperty>(this IEnumerable<T> source, Func<T, TProperty> getPropertyFunc)
    {
        return source.Distinct(new PropertyEqualityComparer<T, TProperty>(getPropertyFunc));
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> emptyExceptionHandler, Func<Exception> manyExceptionHandler)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (emptyExceptionHandler == null) throw new ArgumentNullException(nameof(emptyExceptionHandler));
        if (manyExceptionHandler == null) throw new ArgumentNullException(nameof(manyExceptionHandler));

        return source.Where(predicate).Single(emptyExceptionHandler, manyExceptionHandler);
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> emptyExceptionHandler)
    {
        return source.Single(predicate, emptyExceptionHandler, () => new InvalidOperationException("More Than One Element"));
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler)
    {
        return source.Single(emptyExceptionHandler, () => new InvalidOperationException("More Than One Element"));
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler, Func<Exception> manyExceptionHandler)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (emptyExceptionHandler == null)
        {
            throw new ArgumentNullException(nameof(emptyExceptionHandler));
        }

        if (manyExceptionHandler == null)
        {
            throw new ArgumentNullException(nameof(manyExceptionHandler));
        }

        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                throw emptyExceptionHandler();
            }

            var current = enumerator.Current;

            if (enumerator.MoveNext())
            {
                throw manyExceptionHandler();
            }

            return current;
        }
    }

    public static T Aggregate<T>(this IEnumerable<Func<T, T>> source, T startElement)
    {
        return source.Aggregate(startElement, (v, f) => f(v));
    }

    public static TResult Partial<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, bool> firstResultPredicate, Func<TSource, bool> secondResultPredicate, Func<List<TSource>, List<TSource>, IList<TSource>, TResult> selector)
    {
        var l1 = new List<TSource>();
        var l2 = new List<TSource>();
        var l3 = new List<TSource>();

        foreach (var item in source)
        {
            if (firstResultPredicate(item))
            {
                l1.Add(item);
            }
            else
            {
                if (secondResultPredicate(item))
                {
                    l2.Add(item);
                }
                else
                {
                    l3.Add(item);
                }
            }
        }

        return selector(l1, l2, l3);
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
        where T : notnull
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
        where TKey : notnull
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

    public static TSource? SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> manyExceptionHandler)
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

    public static IEnumerable<T> MaybeYield<T>(this T? source)
        where T : class
    {
        if (source != null)
        {
            yield return source;
        }
    }

    private class PairGrouping<TKey, TElement>(IList<TElement> list, TKey key)
        : ReadOnlyCollection<TElement>(list), IGrouping<TKey, TElement>
    {
        public TKey Key { get; private set; } = key;
    }

}
