using System.Collections.ObjectModel;

namespace Framework.Core;

public static class DictionaryExtensions
{
    public static void Set<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.ContainsKey(key))
        {
            source[key] = value;
        }
        else
        {
            source.Add(key, value);
        }
    }

    public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dict)
    {
        return dict.ToDictionary(pair => pair.Value, pair => pair.Key);
    }

    public static IEnumerable<IGrouping<TKey, TElement>> ToGroup<TKey, TElement>(this IReadOnlyDictionary<TKey, IEnumerable<TElement>> dict)
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));

        return from pair in dict
               select pair.ToGroup();
    }

    public static Dictionary<TKey, TValue> Concat<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, IReadOnlyDictionary<TKey, TValue> other)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (other == null) throw new ArgumentNullException(nameof(other));

        return ((IEnumerable<KeyValuePair<TKey, TValue>>)source).Concat(other).ToDictionary();
    }

    public static Dictionary<TKey, TNewValue> ChangeValue<TKey, TOldValue, TNewValue>(this IReadOnlyDictionary<TKey, TOldValue> source, Func<TOldValue, TNewValue> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToDictionary(pair => pair.Key, pair => selector(pair.Value));
    }

    public static Dictionary<TNewKey, TValue> ChangeKey<TOldKey, TNewKey, TValue>(this IReadOnlyDictionary<TOldKey, TValue> source, Func<TOldKey, TNewKey> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.ToDictionary(pair => selector(pair.Key), pair => pair.Value);
    }

    public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToDictionary();
    }

    public static TValue GetValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<Exception> getKeyNotFoundError)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getKeyNotFoundError == null) throw new ArgumentNullException(nameof(getKeyNotFoundError));

        return source.GetValueOrDefault(key, () => { throw getKeyNotFoundError(); });
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, TValue defaultValue)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

        return source.GetValueOrDefault(key, () => defaultValue);
    }

    public static TValue GetValueOrDefault<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source, TKey key, Func<TValue> getDefaultValueFunc)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getDefaultValueFunc == null) throw new ArgumentNullException(nameof(getDefaultValueFunc));

        TValue value;

        return source.TryGetValue(key, out value) ? value : getDefaultValueFunc();
    }

    public static ReadOnlyDictionary<TKey, TSource> ToReadOnlyDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.ToReadOnlyDictionary(keySelector, v => v);
    }

    public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> elementSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary(keySelector, elementSelector));
    }

    public static SortedDictionary<TKey, TValue> AsSorted<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new SortedDictionary<TKey, TValue>(source.Clone());
    }

    public static Dictionary<TKey, TValue> AggregateValues<TKey, TValue>(this IEnumerable<IReadOnlyDictionary<TKey, TValue>> dicts, Func<TKey, TValue, TValue, TValue> aggregateFunc)
    {
        if (dicts == null) throw new ArgumentNullException(nameof(dicts));
        if (aggregateFunc == null) throw new ArgumentNullException(nameof(aggregateFunc));

        var request = from dict in dicts
                      from pair in dict
                      group pair.Value by pair.Key;

        return request.ToDictionary(pair => pair.Key, pair => pair.Aggregate((state, value) => aggregateFunc(pair.Key, state, value)));
    }

    public static Dictionary<TKey, int> SumValues<TKey>(this IEnumerable<IReadOnlyDictionary<TKey, int>> dicts)
    {
        if (dicts == null) throw new ArgumentNullException(nameof(dicts));

        return dicts.AggregateValues((v1, v2) => v1 + v2);
    }

    public static Dictionary<TKey, decimal> SumValues<TKey>(this IEnumerable<IReadOnlyDictionary<TKey, decimal>> dicts)
    {
        if (dicts == null) throw new ArgumentNullException(nameof(dicts));

        return dicts.AggregateValues((v1, v2) => v1 + v2);
    }

    public static Dictionary<TKey, decimal> MulValues<TKey>(this IEnumerable<IReadOnlyDictionary<TKey, decimal>> dicts)
    {
        if (dicts == null) throw new ArgumentNullException(nameof(dicts));

        return dicts.AggregateValues((v1, v2) => v1 * v2);
    }

    public static Dictionary<TKey, TValue> AggregateValues<TKey, TValue>(this IEnumerable<IReadOnlyDictionary<TKey, TValue>> dicts, Func<TValue, TValue, TValue> aggregateFunc)
    {
        if (dicts == null) throw new ArgumentNullException(nameof(dicts));
        if (aggregateFunc == null) throw new ArgumentNullException(nameof(aggregateFunc));

        var request = from dict in dicts
                      from pair in dict
                      group pair.Value by pair.Key;

        return request.ToDictionary(pair => pair.Key, pair => pair.Aggregate(aggregateFunc));
    }

    public static T[] ToArrayI<T>(this IReadOnlyDictionary<int, T> dict)
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));

        return Enumerable.Range(0, dict.Count).ToArray(i => dict[i]);
    }

    public static IReadOnlyDictionary<TKey, TNewValue> ToCovarianceReadOnlyDictionary<TKey, TNewValue, TOldValue>(this IReadOnlyDictionary<TKey, TOldValue> dict, Func<TOldValue, TNewValue> selector)
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new CovarianceReadOnlyDictionary<TKey, TNewValue, TOldValue>(dict, selector);
    }

    public static IReadOnlyDictionary<TKey, TNewValue> ToCovarianceReadOnlyDictionary<TKey, TNewValue, TOldValue>(this IReadOnlyDictionary<TKey, TOldValue> dict)
            where TOldValue : TNewValue
    {
        if (dict == null) throw new ArgumentNullException(nameof(dict));

        return dict.ToCovarianceReadOnlyDictionary(v => (TNewValue) v);
    }

    private class CovarianceReadOnlyDictionary<TKey, TNewValue, TOldValue> : IReadOnlyDictionary<TKey, TNewValue>
    {
        private readonly IReadOnlyDictionary<TKey, TOldValue> _dict;

        private readonly Func<TOldValue, TNewValue> _selector;

        public CovarianceReadOnlyDictionary(IReadOnlyDictionary<TKey, TOldValue> dict, Func<TOldValue, TNewValue> selector)
        {
            if (dict == null) throw new ArgumentNullException(nameof(dict));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            this._dict = dict;
            this._selector = selector;
        }

        public IEnumerator<KeyValuePair<TKey, TNewValue>> GetEnumerator()
        {
            return this._dict.Select(pair => new KeyValuePair<TKey, TNewValue>(pair.Key, this._selector(pair.Value))).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get { return this._dict.Count; }
        }

        public bool ContainsKey(TKey key)
        {
            return this._dict.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TNewValue value)
        {
            TOldValue oldValue;

            if (this._dict.TryGetValue(key, out oldValue))
            {
                value = this._selector(oldValue);
                return true;
            }
            else
            {
                value = default(TNewValue);
                return false;
            }
        }

        public TNewValue this[TKey key]
        {
            get
            {
                return this._selector(this._dict[key]);
            }
        }

        public IEnumerable<TKey> Keys
        {
            get { return this._dict.Keys; }
        }

        public IEnumerable<TNewValue> Values
        {
            get { return this._dict.Values.Select(this._selector); }
        }
    }
}

public static class DictionaryHelper
{
    public static Dictionary<TKey, TValue> Create<TKey, TValue>(params KeyValuePair<TKey, TValue>[] pairs)
    {
        return pairs.ToDictionary();
    }
}
