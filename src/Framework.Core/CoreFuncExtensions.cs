using CommonFramework;

namespace Framework.Core;

public static class CoreFuncExtensions
{
    public static Func<TArg1, TArg2, IEnumerable<TResult>> Sum<TArg1, TArg2, TResult>(this IEnumerable<Func<TArg1, TArg2, IEnumerable<TResult>>> funcs)
    {
        if (funcs == null) throw new ArgumentNullException(nameof(funcs));

        return funcs.Aggregate(FuncHelper.Create((TArg1 _, TArg2 __) => Enumerable.Empty<TResult>()), (f1, f2) => (arg1, arg2) => f1(arg1, arg2).Concat(f2(arg1, arg2)));
    }

    public static TInterface ToLazyInterfaceImplement<TInterface>(this Func<TInterface> getValueFunc)
    {
        if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));

        return getValueFunc.ToLazyInterfaceImplement<TInterface, TInterface>();
    }

    public static TInterface ToLazyInterfaceImplement<TInterface, TImplement>(this Func<TImplement> getValueFunc)
        where TImplement : TInterface
    {
        return LazyInterfaceImplementHelper<TInterface>.CreateProxy(() => getValueFunc());
    }

    public static Func<TArg, TResult> WithCache<TArg, TResult>(this Func<TArg, TResult> func, IEqualityComparer<TArg>? equalityComparer = null)
        where TArg : notnull
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var cache = new Dictionary<TArg, TResult>(equalityComparer ?? EqualityComparer<TArg>.Default);

        return arg => cache.GetValueOrCreate(arg, () => func(arg));
    }

    public static Func<TArg, TResult> WithLock<TArg, TResult>(this Func<TArg, TResult> func, object? baseLocker = null)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

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
