using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Framework.Core;

public static class CoreEnumerableExtensions
{
    /// <summary>
    /// Перебор элементов коллекции до условия
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source">источник</param>
    /// <param name="predicate">фильтр</param>
    /// <param name="includeLast">флаг включения последнего элемента (на котором фильтрация прекратилась) в выборку</param>
    /// <returns></returns>
    public static IEnumerable<TSource> TakeWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool includeLast)
    {
        foreach (var value in source)
        {
            if (predicate(value))
            {
                yield return value;
            }
            else
            {
                if (includeLast)
                {
                    yield return value;
                    yield break;
                }
            }
        }
    }

    public static bool SequenceEqual<TSource, TOther>(this IEnumerable<TSource> first, IEnumerable<TOther> second, Func<TSource, TOther, bool> compareFunc)
    {
        using (var enumerator1 = first.GetEnumerator())
        {
            using (var enumerator2 = second.GetEnumerator())
            {
                while (enumerator1.MoveNext())
                {
                    if (!enumerator2.MoveNext() || !compareFunc(enumerator1.Current, enumerator2.Current))
                        return false;
                }
                if (enumerator2.MoveNext())
                    return false;
            }
        }
        return true;
    }

    public static T Aggregate<T>(this IEnumerable<Func<T, T>> source, T startElement)
    {
        return source.Aggregate(startElement, (v, f) => f(v));
    }

    public static bool HasAttribute<T>(this IEnumerable<Attribute> source)
            where T : Attribute
    {
        return source.OfType<T>().Any();
    }

    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return !source.Any();
    }

    public static IEnumerable<T> GetDuplicates<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        var hashSet = new HashSet<T>(comparer ?? EqualityComparer<T>.Default);

        return source.Where(item => !hashSet.Add(item));
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

    public static IEnumerable<T> GetGraphElements<T>(this T source, Func<T, IEnumerable<T>> getChildFunc, bool skipFirstElement = false, IEqualityComparer<T> comparer = null)
    {
        if (null == getChildFunc) throw new ArgumentNullException(nameof(getChildFunc));

        var hashSet = new HashSet<T>(comparer ?? EqualityComparer<T>.Default);

        return source.GetAllElements(c => getChildFunc(c).Where(hashSet.Add), skipFirstElement);
    }

    public static void Override<T>(this ICollection<T> source, IEnumerable<T> newItems)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (newItems == null) throw new ArgumentNullException(nameof(newItems));

        source.Clear();

        newItems.Foreach(source.Add);
    }

    public static IEnumerable<T> GetAllElements<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> getChildFunc)
    {
        if (null == getChildFunc)
        {
            throw new ArgumentNullException(nameof(getChildFunc));
        }

        return source.SelectMany(child => child.GetAllElements(getChildFunc));
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
                        throw new System.ArgumentException("Unexpected first element", nameof(source));
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
                    throw new System.ArgumentException("EmptyCollection", nameof(source));
                }
            }
        }
    }

    public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, T expectedElement, IEqualityComparer<T> equalityComparer, bool raiseIfNotEquals = false)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (equalityComparer == null) throw new ArgumentNullException(nameof(equalityComparer));

        using (var enumerator = source.GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                var lastElement = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    yield return lastElement;
                    lastElement = enumerator.Current;
                }

                if (!equalityComparer.Equals(lastElement, expectedElement))
                {
                    if (raiseIfNotEquals)
                    {
                        throw new System.ArgumentException("Unexpected last element", nameof(source));
                    }

                    yield return lastElement;
                }
            }
            else
            {
                if (raiseIfNotEquals)
                {
                    throw new System.ArgumentException("EmptyCollection", nameof(source));
                }
            }
        }
    }

    public static TResult IfNull<TResult>(this TResult source, Func<TResult> otherResultFunc)
            where TResult : class
    {
        return source ?? otherResultFunc();
    }

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> equalsFunc, Func<T, int> getHashFunc = null)
    {
        return source.Distinct(new EqualityComparerImpl<T>(equalsFunc, getHashFunc));
    }

    public static IEnumerable<T> Distinct<T, TProperty>(this IEnumerable<T> source, Func<T, TProperty> getPropertyFunc)
    {
        return source.Distinct(new PropertyEqualityComparer<T, TProperty>(getPropertyFunc));
    }

    public static IEnumerable<TSource> Except<TSource, TOther>(this IEnumerable<TSource> source, IEnumerable<TOther> other, Func<TSource, TOther, bool> equalsFunc)
    {
        var otherCache = other.ToArray();

        return source.Where(sourceItem => !otherCache.Any(otherItem => equalsFunc(sourceItem, otherItem)));
    }

    public static bool SequenceEqual<T>(this IEnumerable<T> source, IEnumerable<T> other, Func<T, T, bool> equalsFunc, Func<T, int> getHashFunc = null)
    {
        return source.SequenceEqual(other, new EqualityComparerImpl<T>(equalsFunc, getHashFunc));
    }

    public static void RemoveBy<T>(this ICollection<T> source, Func<T, bool> selector)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (selector == null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        source.Where(selector).ToList().ForEach(v => source.Remove(v));
    }

    public static void ClearWithDispose(this IList<IDisposable> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        source.Foreach(v => v.Dispose());
        source.Clear();

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

    public static T FirstOr<T>(this IEnumerable<T> source, Func<T> func)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (func == null) throw new ArgumentNullException(nameof(func));

        using (var enumerator = source.GetEnumerator())
        {
            return enumerator.MoveNext() ? enumerator.Current : func();
        }
    }

    public static T LastOr<T>(this IEnumerable<T> source, Func<T> func)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (func == null) throw new ArgumentNullException(nameof(func));

        return source.Aggregate(func, (_, v) => () => v)();
    }
#nullable enable
    /// <summary>
    /// Merge Target collection into Source collection.
    /// Call <see cref="createFunc"/> for not existing objects in Source
    /// Call <see cref="removeAction"/> for exists in Source but not exists in Target
    /// Call <see cref="mapAction"/> for exists in both collection and created items
    /// </summary>
    public static void Merge<TSource, TTarget, TKey>(
            this IEnumerable<TSource> source,
            IEnumerable<TTarget> target,
            Func<TTarget, TKey> getTargetKey,
            Func<TSource, TKey> getSourceKey,
            Func<TTarget, TSource> createFunc,
            Action<IEnumerable<TSource>> removeAction,
            Action<TTarget, TSource> mapAction,
            Func<TKey, bool>? isDefaultKey = null)
            where TKey : notnull
    {
        var isDefaultKeyF = isDefaultKey ?? (v => v.IsDefault());

        var targetMap = target.ToDictionary(
                                            s => isDefaultKeyF(getTargetKey(s))
                                                         ? new object()
                                                         : getTargetKey(s));
        var removed = new List<TSource>();

        foreach (var sourceItem in source.ToList())
        {
            var key = getSourceKey(sourceItem);
            if (targetMap.TryGetValue(key, out var targetItem))
            {
                mapAction(targetItem, sourceItem);
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
            var targetItem = createFunc(sourceItem);
            mapAction(sourceItem, targetItem);
        }
    }
#nullable disable
    public static void Merge<T, S, TKey>(
            this IEnumerable<T> source,
            IEnumerable<S> target,
            Func<S, TKey> getSKey,
            Func<T, TKey> getTKey,
            Func<S, T> createAndMapFunc,
            Action<IEnumerable<T>> removeAction,
            Func<TKey, bool> isDefaultKey = null)
    {
        var isDefaultKeyF = isDefaultKey ?? (v => v.IsDefault());

        var targetMap = target.ToDictionary(s => isDefaultKeyF(getSKey(s)) ? new object() : getSKey(s));

        var removed = new List<T>();

        foreach (var sourceItem in source.ToList())
        {
            S targetItem;
            var key = getTKey(sourceItem);
            if (targetMap.TryGetValue(key, out targetItem))
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

    /// <summary>
    /// Приведение к массиву
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="checkNullElements">Проверять элементы на нулевые значения</param>
    /// <returns></returns>
    public static T[] ToArray<T>(this IEnumerable<T> source, bool checkNullElements)
            where T : class
    {
        var array = source.ToArray();

        if (checkNullElements && Enumerable.Contains(array, null))
        {
            throw new System.ArgumentException("Collection contains null value", nameof(source));
        }

        return array;
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="emptyExceptionHandler"></param>
    /// <returns></returns>
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

    public static TSource Last<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler)
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
            if (enumerator.MoveNext())
            {
                var current = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    current = enumerator.Current;
                }

                return current;
            }
            else
            {
                throw emptyExceptionHandler();
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="emptyExceptionHandler"></param>
    /// <returns></returns>
    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler)
    {
        return source.Single(emptyExceptionHandler, () => new InvalidOperationException("More Than One Element"));
    }

    public static Maybe<TSource> SingleMaybe<TSource>(this IEnumerable<TSource> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                return Maybe<TSource>.Nothing;
            }

            var value = enumerator.Current;

            return Maybe.OfCondition(!enumerator.MoveNext(), () => value);
        }
    }

    public static Maybe<TSource> SingleMaybe<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));

        return source.Where(predicate).SingleMaybe();
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

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<Exception> manyExceptionHandler)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (manyExceptionHandler == null) throw new ArgumentNullException(nameof(manyExceptionHandler));

        using (var enumerator = source.GetEnumerator())
        {
            if (!enumerator.MoveNext())
            {
                return default(TSource);
            }

            var current = enumerator.Current;

            if (enumerator.MoveNext())
            {
                throw manyExceptionHandler();
            }

            return current;
        }
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

    public static IEnumerable<TResult> OfTypeStrong<TSource, TResult>(this IEnumerable<TSource> source)
            where TResult : TSource
    {
        return source.OfType<TResult>();
    }

    public static IEnumerable<TResult> CastStrong<TSource, TResult>(this IEnumerable<TSource> source)
            where TResult : TSource
    {
        return source.Cast<TResult>();
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

    public static int IndexOf<TSource>(this IEnumerable<TSource> source, TSource element, IEqualityComparer<TSource> comparer = null)
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

    public static IEnumerable<TSource> RepeatInfinity<TSource>(this TSource v)
    {
        while (true)
        {
            yield return v;
        }
    }

    public static IEnumerable<int> RangeInfinity(this int startIndex)
    {
        while (true)
        {
            yield return startIndex++;
        }
    }

    public static IEnumerable<TResult> NullIfEmpty<TResult>(this IEnumerable<TResult> source)
    {
        if (source == null) return null;

        var cache = source.ToArray();

        return cache.Any() ? cache : null;
    }

    public static TResult[] NullIfEmpty<TResult>(this TResult[] source)
    {
        if (source == null) return null;

        return source.Any() ? source : null;
    }

    public static IEnumerable<T> MaybeYield<T>(this T source)
            where T : class
    {
        if (source != null)
        {
            yield return source;
        }
    }

    public static IEnumerable<TResult> MaybeToCollection<TSource, TResult>(this TSource source, Func<TSource, IEnumerable<TResult>> func)
            where TSource : class
    {
        if (func == null) { throw new ArgumentNullException(nameof(func)); }

        return source.Maybe(func).EmptyIfNull();
    }

    public static void RemoveRange<T>(this IList<T> source, IEnumerable<T> items)
    {
        items.Foreach(item => source.Remove(item));
    }

    public static IEnumerable<ValueTuple<T1, T2>> GetCombineItems<T1, T2, TKey>(this IEnumerable<T1> source1, IEnumerable<T2> source2, Func<T1, TKey> key1Selector, Func<T2, TKey> key2Selector, Func<MergeResult<T1, T2>, Exception> getNonCombineItemsException)
    {
        if (source1 == null) throw new ArgumentNullException(nameof(source1));
        if (source2 == null) throw new ArgumentNullException(nameof(source2));
        if (key1Selector == null) throw new ArgumentNullException(nameof(key1Selector));
        if (key2Selector == null) throw new ArgumentNullException(nameof(key2Selector));
        if (getNonCombineItemsException == null) throw new ArgumentNullException(nameof(getNonCombineItemsException));

        var mergeResult = source1.GetMergeResult(source2, key1Selector, key2Selector);

        if (mergeResult.AddingItems.Any() || mergeResult.RemovingItems.Any())
        {
            throw getNonCombineItemsException(mergeResult);
        }
        else
        {
            return mergeResult.CombineItems;
        }
    }

    public static MergeResult<T, T> GetMergeResult<T>(this IEnumerable<T> source, IEnumerable<T> target, Func<T, T, bool> equalsFunc)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (target == null) throw new ArgumentNullException(nameof(target));

        return source.GetMergeResult(target, v => v, v => v, equalsFunc);
    }

    public static MergeResult<TSource, TTarget> GetMergeResult<TSource, TTarget, TKey>(
            this IEnumerable<TSource> source,
            IEnumerable<TTarget> target,
            Func<TSource, TKey> sourceKeySelector,
            Func<TTarget, TKey> targetKeySelector,
            Func<TKey, TKey, bool> equalsFunc)
    {
        if (equalsFunc == null) throw new ArgumentNullException(nameof(equalsFunc));

        return source.GetMergeResult(target, sourceKeySelector, targetKeySelector, new EqualityComparerImpl<TKey>(equalsFunc, _ => 0));
    }

    public static MergeResult<T, T> GetMergeResult<T>(this IEnumerable<T> source, IEnumerable<T> target, IEqualityComparer<T> comparer = null)
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
            IEqualityComparer<TKey> comparer = null)
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
            TTarget targetItem;

            var sourceKey = sourceKeySelector(sourceItem);

            if (targetMap.TryGetValue(sourceKey, out targetItem))
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

    public static IEnumerable<ValueTuple<T, K>> UnionByIndex<T, K>(this IEnumerable<T> source, IEnumerable<K> other)
    {
        return source.Zip(other, ValueTuple.Create);
    }

    public static IEnumerable<T> CheckNotNull<T>(this IEnumerable<T> source)
            where T : class
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Select(v => v.FromMaybe(() => new System.ArgumentException("source contains null value", nameof(source))));
    }

    public static bool IsIntersected<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));

        return source.IsIntersected(other, EqualityComparer<TSource>.Default);
    }

    public static bool IsIntersected<TSource>(this IEnumerable<TSource> source, IEnumerable<TSource> other, IEqualityComparer<TSource> comparer)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));
        if (comparer == null) throw new ArgumentNullException(nameof(comparer));

        return source.Intersect(other, comparer).Any();
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

    public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> source, int size)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (size < 1) throw new System.ArgumentException("count");

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

    public static IEnumerable<T> SkipElement<T>(this IEnumerable<T> source, int index)
    {
        return source.Where((v, i) => i != index);
    }

    public static IEnumerable<T> CollectMaybe<T>(this IEnumerable<Maybe<T>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return from item in source
               let just = item as Just<T>
               where just != null
               select just.Value;
    }

    public static IEnumerable<T> CollectMaybe<T>(this IEnumerable<Func<Maybe<T>>> source)
    {
        return source.Select(f => f()).CollectMaybe();
    }

    public static Maybe<IEnumerable<T>> CollectJust<T>(this IEnumerable<Maybe<T>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Aggregate(Maybe.Return(Enumerable.Empty<T>()), (maybeState, maybeValue) => from state in maybeState
                                                                         from value in maybeValue
                                                                         select state.Concat(new[] { value }));
    }

    /// <summary>
    /// Runs safe integer sum calculation over IQueryable using selector specified. Method doesn't throw exception
    /// even if query returns nothing.
    /// </summary>
    public static int SafeSum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, int?>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Sum(selector).GetValueOrDefault();
    }

    /// <summary>
    /// Runs safe long sum calculation over IQueryable using selector specified. Method doesn't throw exception
    /// even if query returns nothing.
    /// </summary>
    public static long SafeSum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, long?>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Sum(selector).GetValueOrDefault();
    }

    /// <summary>
    /// Runs safe float sum calculation over IQueryable using selector specified. Method doesn't throw exception
    /// even if query returns nothing.
    /// </summary>
    public static float SafeSum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, float?>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Sum(selector).GetValueOrDefault();
    }

    /// <summary>
    /// Runs safe double sum calculation over IQueryable using selector specified. Method doesn't throw exception
    /// even if query returns nothing.
    /// </summary>
    public static double SafeSum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, double?>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Sum(selector).GetValueOrDefault();
    }

    /// <summary>
    /// Runs safe decimal sum calculation over IQueryable using selector specified. Method doesn't throw exception
    /// even if query returns nothing.
    /// </summary>
    public static decimal SafeSum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Sum(selector).GetValueOrDefault();
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

    public static IGrouping<TNewKey, TElement> ChangeKey<TOldKey, TNewKey, TElement>(this IGrouping<TOldKey, TElement> source, Func<TOldKey, TNewKey> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new PairGrouping<TNewKey, TElement>(source.ToList(), selector(source.Key));
    }

    public static IGrouping<TKey, TNewElement> ChangeValues<TKey, TOldElement, TNewElement>(this IGrouping<TKey, TOldElement> source, Func<TOldElement, TNewElement> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new PairGrouping<TKey, TNewElement>(source.ToList(selector), source.Key);
    }

    private class PairGrouping<TKey, TElement> : ReadOnlyCollection<TElement>, IGrouping<TKey, TElement>
    {
        public PairGrouping(IList<TElement> list, TKey key)
                : base(list)
        {
            this.Key = key;
        }

        public TKey Key { get; private set; }
    }

    public static decimal Mul(this IEnumerable<decimal> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Aggregate((v1, v2) => v1 * v2);
    }

    public static decimal Sum(this IEnumerable<decimal> values, Func<decimal, decimal, decimal> sumFunc)
    {
        if (values == null) throw new ArgumentNullException(nameof(values));

        return sumFunc == null ? values.Sum() : values.Aggregate(0, sumFunc);
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<Exception> emptyExceptionHandler, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        if (emptyExceptionHandler == null) throw new ArgumentNullException(nameof(emptyExceptionHandler));
        if (manyExceptionHandler == null) throw new ArgumentNullException(nameof(manyExceptionHandler));

        return source.Where(predicate).Single(emptyExceptionHandler, manyExceptionHandler);
    }

    public static TSource Single<TSource>(this IEnumerable<TSource> source, Func<Exception> emptyExceptionHandler, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (manyExceptionHandler == null) throw new ArgumentNullException(nameof(manyExceptionHandler));

        var items = source.ToList();

        switch (items.Count)
        {
            case 0:
                throw emptyExceptionHandler();

            case 1:
                return items[0];

            default:
                throw manyExceptionHandler(items);
        }
    }

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
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

    public static TSource SingleOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, Func<IReadOnlyCollection<TSource>, Exception> manyExceptionHandler)
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

    public static IReadOnlyList<T> ToReadOnlyListI<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToList();
    }

    public static IReadOnlyList<TResult> ToReadOnlyListI<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Select(selector).ToReadOnlyListI();
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollectionI<T>(this IEnumerable<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToList();
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionaryI<TValue, TKey>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

        return source.ToReadOnlyDictionary(keySelector);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionaryI<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToReadOnlyDictionary(keySelector, valueSelector);
    }

    public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionaryI<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}
