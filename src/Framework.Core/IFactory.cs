using Anch.Core;

namespace Framework.Core;

public interface IFactory<in TArg, out TResult>
{
    TResult Create(TArg arg);
}

public interface IFactory<in TArg1, in TArg2, out TResult>
{
    TResult Create(TArg1 arg1, TArg2 arg2);
}

public static class FactoryExtensions
{
    public static IFactory<TArg, TResult> WithLock<TArg, TResult>(this IFactory<TArg, TResult> factory)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        return new FuncFactory<TArg, TResult>(new Func<TArg, TResult>(factory.Create).WithLock());
    }

    public static IFactory<TArg, TResult> WithCache<TArg, TResult>(this IFactory<TArg, TResult> factory, IEqualityComparer<TArg> equalityComparer = null)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        return new FuncFactory<TArg, TResult>(new Func<TArg, TResult>(factory.Create).WithCache(equalityComparer));
    }

    public static IFactory<T> WithCache<T>(this IFactory<T> factory, bool createLazy = true)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));

        if (createLazy)
        {
            var lazyInstance = LazyHelper.Create(factory.Create);

            return new FuncFactory<T>(() => lazyInstance.Value);
        }
        else
        {
            var intance = factory.Create();

            return new FuncFactory<T>(() => intance);
        }
    }

    public static IFactory<TSource, TNewResult> Select<TSource, TResult, TNewResult>(this IFactory<TSource, TResult> factory, Func<TSource, TNewResult> selector)
    {
        if (factory == null) throw new ArgumentNullException(nameof(factory));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new FuncFactory<TSource, TNewResult>(selector);
    }
}

public class FuncFactory<T>(Func<T> createFunc) : IFactory<T>
{
    public T Create() => createFunc();
}

public class FuncFactory<TArg, TResult>(Func<TArg, TResult> createFunc) : IFactory<TArg, TResult>
{
    public TResult Create(TArg arg) => createFunc(arg);
}

public class FuncFactory<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> createFunc) : IFactory<TArg1, TArg2, TResult>
{
    public TResult Create(TArg1 arg1, TArg2 arg2) => createFunc(arg1, arg2);
}
