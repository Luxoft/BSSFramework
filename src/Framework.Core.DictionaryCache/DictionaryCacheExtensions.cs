namespace Framework.Core;

public static class DictionaryCacheExtensions
{
    public static TValue GetValue<TK1, TValue>(this IDictionaryCache<TK1, TValue> cache, TK1 tk1)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache[tk1];
    }

    public static TValue GetValue<TK1, TK2, TValue>(this IDictionaryCache<Tuple<TK1, TK2>, TValue> cache, TK1 tk1, TK2 tk2)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache[Tuple.Create(tk1, tk2)];
    }

    public static TValue GetValue<TK1, TK2, TK3, TValue>(this IDictionaryCache<Tuple<TK1, TK2, TK3>, TValue> cache, TK1 tk1, TK2 tk2, TK3 tk3)
    {
        if (cache == null) throw new ArgumentNullException(nameof(cache));

        return cache[Tuple.Create(tk1, tk2, tk3)];
    }


    public static IDictionaryCache<TKey, TValue> WithLock<TKey, TValue>(this IDictionaryCache<TKey, TValue> dictionaryCache, object locker = null)
    {
        if (dictionaryCache == null) throw new ArgumentNullException(nameof(dictionaryCache));

        return new ConcurrentDictionaryCache<TKey, TValue>(dictionaryCache, locker ?? new object());
    }

    public static IDictionaryCache<TKey, TValue> ToCache<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return new DictionaryCache<TKey, TValue>(key => dictionary[key], dictionary.Comparer);
    }


    private class ConcurrentDictionaryCache<TKey, TValue> : IDictionaryCache<TKey, TValue>
    {
        private readonly IDictionaryCache<TKey, TValue> _baseDictionaryCache;

        private readonly object _locker;


        public ConcurrentDictionaryCache(IDictionaryCache<TKey, TValue> baseDictionaryCache, object locker)
        {
            this._baseDictionaryCache = baseDictionaryCache ?? throw new ArgumentNullException(nameof(baseDictionaryCache));
            this._locker = locker ?? throw new ArgumentNullException(nameof(locker));
        }


        public TValue this[TKey key]
        {
            get
            {
                lock (this._locker)
                {
                    return this._baseDictionaryCache[key];
                }
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                lock (this._locker)
                {
                    return this._baseDictionaryCache.Values.ToArray();
                }
            }
        }
    }
}
