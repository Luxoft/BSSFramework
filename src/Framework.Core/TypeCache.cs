using System.Collections;

namespace Framework.Core;

public abstract class TypeCache
{
    private readonly object _rootCacheLocker = new object();

    private readonly object _argsCacheLocker = new object();

    private readonly Dictionary<Type, object> _rootCache = new Dictionary<Type, object>();

    private readonly Dictionary<Type, Tuple<object, IDictionary>> _argsCache = new Dictionary<Type, Tuple<object, IDictionary>>();


    protected TValue GetValue<TValue>(Func<TValue> createFunc)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return (TValue)this._rootCache.GetValueOrCreate(typeof(TValue), this._rootCacheLocker, () => createFunc());
    }

    protected TValue GetValue<TValue, TArg>(Func<TValue> createFunc, TArg arg, IEqualityComparer<TArg> argComparer = null)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        return this._argsCache.GetValueOrCreate(typeof(TValue), this._argsCacheLocker, () => new Tuple<object, IDictionary>(new object(), new Dictionary<TArg, TValue>(argComparer))).Pipe(t =>
        {
            var cache = (Dictionary<TArg, TValue>)t.Item2;

            return cache.GetValueOrCreate(arg, t.Item1, createFunc);
        });
    }
}
