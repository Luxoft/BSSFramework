using Anch.Core;

namespace Framework.Core;

public static class CoreFuncExtensions
{
    public static Func<TArg1, TArg2, IEnumerable<TResult>> Sum<TArg1, TArg2, TResult>(this IEnumerable<Func<TArg1, TArg2, IEnumerable<TResult>>> funcs)
    {
        if (funcs is null) throw new ArgumentNullException(nameof(funcs));

        return funcs.Aggregate(FuncHelper.Create((TArg1 _, TArg2 __) => Enumerable.Empty<TResult>()), (f1, f2) => (arg1, arg2) => f1(arg1, arg2).Concat(f2(arg1, arg2)));
    }

    public static TInterface ToLazyInterfaceImplement<TInterface>(this Func<TInterface> getValueFunc)
    {
        if (getValueFunc is null) throw new ArgumentNullException(nameof(getValueFunc));

        return getValueFunc.ToLazyInterfaceImplement<TInterface, TInterface>();
    }

    public static TInterface ToLazyInterfaceImplement<TInterface, TImplement>(this Func<TImplement> getValueFunc)
        where TImplement : TInterface =>
        LazyInterfaceImplementHelper<TInterface>.CreateProxy(() => getValueFunc());

    /// <summary>
    /// Memoizes <paramref name="func"/> by argument. The cache is a plain <see cref="Dictionary{TKey,TValue}"/> and is NOT thread-safe:
    /// for shared use combine with <see cref="WithLock{TArg,TResult}"/> and apply the lock last (<c>func.WithCache().WithLock()</c>),
    /// otherwise the cache is accessed outside the lock. Results are never evicted.
    /// </summary>
    public static Func<TArg, TResult> WithCache<TArg, TResult>(this Func<TArg, TResult> func, IEqualityComparer<TArg>? equalityComparer = null)
        where TArg : notnull
    {
        if (func is null) throw new ArgumentNullException(nameof(func));

        var cache = new Dictionary<TArg, TResult>(equalityComparer ?? EqualityComparer<TArg>.Default);

        return arg => cache.GetValueOrCreate(arg, () => func(arg));
    }

    /// <summary>
    /// Serializes calls through a single lock. If no <paramref name="baseLocker"/> is passed, a private one is created per wrapper,
    /// so two wrappers over the same resource don't exclude each other unless they share a locker.
    /// The lock is held while <paramref name="func"/> runs: don't call back into the wrapper from <paramref name="func"/> on another thread.
    /// </summary>
    public static Func<TArg, TResult> WithLock<TArg, TResult>(this Func<TArg, TResult> func, object? baseLocker = null)
    {
        if (func is null) throw new ArgumentNullException(nameof(func));

        var locker = baseLocker ?? new object();

        return arg =>
               {
                   lock (locker)
                   {
                       return func(arg);
                   }
               };
    }
}
