using CommonFramework;
using CommonFramework.DictionaryCache;

namespace Framework.Core;

public class LazyImplementDictionaryCache<TKey, TValue>(
    Func<TKey, TValue> getGetNewValueFunc,
    Func<TKey, Type>? getProxyType = null,
    IEqualityComparer<TKey>? comparer = null)
    : IDictionaryCache<TKey, TValue>
    where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> dict = new(comparer ?? EqualityComparer<TKey>.Default);

    public TValue this[TKey key]
    {
        get
        {
            if (this.dict.TryGetValue(key, out var result))
            {
                return result;
            }
            else
            {
                var createProxyFunc = this.GetCreateProxyFunc(key);

                var proxy = createProxyFunc(() => result);

                this.dict.Add(key, proxy);

                result = getGetNewValueFunc(key);

                this.dict[key] = result;

                return result;
            }
        }
    }

    private Func<Func<TValue>, TValue> GetCreateProxyFunc(TKey key)
    {
        var proxyType = getProxyType.Maybe(f => f(key));

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


    IEnumerable<TValue> IDictionaryCache<TKey, TValue>.Values => this.dict.Values;
}
