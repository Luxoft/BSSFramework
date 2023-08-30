namespace Framework.Core;

public class LazyImplementDictionaryCache<TKey, TValue> : IDictionaryCache<TKey, TValue>
{
    private readonly Dictionary<TKey, TValue> _dict;

    private readonly Func<TKey, TValue> _getGetNewValueFunc;

    private readonly Func<TKey, Type> _getProxyType;


    public LazyImplementDictionaryCache(Func<TKey, TValue> getGetNewValueFunc, Func<TKey, Type> getProxyType = null, IEqualityComparer<TKey> comparer = null)
    {
        if (getGetNewValueFunc == null) throw new ArgumentNullException(nameof(getGetNewValueFunc));

        this._getGetNewValueFunc = getGetNewValueFunc;
        this._getProxyType = getProxyType;

        this._dict = new Dictionary<TKey, TValue>(comparer ?? EqualityComparer<TKey>.Default);
    }



    public TValue this[TKey key]
    {
        get
        {
            TValue result;

            if (this._dict.TryGetValue(key, out result))
            {
                return result;
            }
            else
            {
                var createProxyFunc = this.GetCreateProxyFunc(key);

                var proxy = createProxyFunc(() => result);

                this._dict.Add(key, proxy);

                result = this._getGetNewValueFunc(key);

                this._dict[key] = result;

                return result;
            }
        }
    }

    private Func<Func<TValue>, TValue> GetCreateProxyFunc(TKey key)
    {
        var proxyType = this._getProxyType.Maybe(f => f(key));

        if (proxyType == null || !proxyType.IsInterface || proxyType == typeof(TValue))
        {
            return LazyInterfaceImplementHelper<TValue>.CreateProxy;
        }
        else
        {
            return new Func<Func<Func<TValue>, TValue>>(GetCreateProxyFunc<TValue>)
                   .CreateGenericMethod(proxyType)
                   .Invoke<Func<Func<TValue>, TValue>>(null);
        }
    }


    private static Func<Func<TValue>, TValue> GetCreateProxyFunc<TProxyValue>()
            where TProxyValue : TValue
    {
        return f => LazyInterfaceImplementHelper<TProxyValue>.CreateProxy(() => (TProxyValue)f());
    }


    IEnumerable<TValue> IDictionaryCache<TKey, TValue>.Values => this._dict.Values;
}
