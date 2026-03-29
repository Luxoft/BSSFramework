using CommonFramework;

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

public class FuncFactory<T> : IFactory<T>
{
    private readonly Func<T> createFunc;


    public FuncFactory(Func<T> createFunc)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        this.createFunc = createFunc;
    }


    public T Create() => this.createFunc();
}

public class FuncFactory<TArg, TResult> : IFactory<TArg, TResult>
{
    private readonly Func<TArg, TResult> createFunc;

    public FuncFactory(Func<TArg, TResult> createFunc)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        this.createFunc = createFunc;
    }

    public TResult Create(TArg arg) => this.createFunc(arg);
}

public class FuncFactory<TArg1, TArg2, TResult> : IFactory<TArg1, TArg2, TResult>
{
    private readonly Func<TArg1, TArg2, TResult> createFunc;

    public FuncFactory(Func<TArg1, TArg2, TResult> createFunc)
    {
        if (createFunc == null) throw new ArgumentNullException(nameof(createFunc));

        this.createFunc = createFunc;
    }

    public TResult Create(TArg1 arg1, TArg2 arg2) => this.createFunc(arg1, arg2);
}
