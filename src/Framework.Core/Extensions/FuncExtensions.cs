namespace Framework.Core;

public static class FuncExtensions
{
    public static Func<T> WithTryFinally<T>(this Func<T> f, Action tryAction, Action finallyAction = null)
    {
        if (f == null) throw new ArgumentNullException(nameof(f));
        if (tryAction == null) throw new ArgumentNullException(nameof(tryAction));

        return () =>
               {
                   tryAction();

                   try
                   {
                       return f();
                   }
                   finally
                   {
                       finallyAction?.Invoke();
                   }
               };
    }

    public static Func<TSource, TResult> ToMaybeFunc<TSource, TResult>(this Func<TSource, TResult> func)
    {
        return func ?? (_ => default(TResult));
    }

    public static TResult MaybeInvoke<TSource, TResult>(this Func<TSource, TResult> func, TSource source)
    {
        return func.ToMaybeFunc()(source);
    }

    public static Func<T> Unwrap<T>(this Lazy<Func<T>> getValue)
    {
        if (getValue == null) throw new ArgumentNullException(nameof(getValue));

        return () => getValue.Value();
    }

    public static Func<TArg, TResult> Unwrap<TArg, TResult>(this Lazy<Func<TArg, TResult>> getValue)
    {
        if (getValue == null) throw new ArgumentNullException(nameof(getValue));

        return v => getValue.Value(v);
    }

    public static Func<TSource, TResult> Combine<TSource, TAccumulate, TResult>(this Func<TSource, TAccumulate> sourceFunc, Func<TAccumulate, TResult> nextFunc)
    {
        if (sourceFunc == null) throw new ArgumentNullException(nameof(sourceFunc));
        if (nextFunc == null) throw new ArgumentNullException(nameof(nextFunc));

        return source => sourceFunc(source).Pipe(nextFunc);
    }

    public static Action<TSource> Combine<TSource, TResult>(this Func<TSource, TResult> sourceFunc, Action<TResult> nextAction)
    {
        if (sourceFunc == null) throw new ArgumentNullException(nameof(sourceFunc));
        if (nextAction == null) throw new ArgumentNullException(nameof(nextAction));

        return source => sourceFunc(source).Pipe(nextAction);
    }

    public static IEnumerable<T> Repeat<T>(this Func<T> createTFunc, int count)
    {
        while (count-- > 0)
        {
            yield return createTFunc();
        }
    }

    public static void Repeat(this Action action, int count)
    {
        while (count-- > 0)
        {
            action();
        }
    }

    public static Lazy<T> ToLazy<T> (this Func<T> getValueFunc)
    {
        if (getValueFunc == null) throw new ArgumentNullException(nameof(getValueFunc));

        return new Lazy<T>(getValueFunc);
    }

    public static Func<TResult> WithCache<TResult>(this Func<TResult> func, bool startAsync = false)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        if (startAsync)
        {
            var locker = new object();
            var waitHandle = new AutoResetEvent(false);

            var result = default(ITryResult<TResult>);

            ThreadPool.QueueUserWorkItem(_ =>
                                         {
                                             result = TryResult.Catch(func);

                                             var handle = waitHandle;
                                             waitHandle = null;
                                             handle.Set();
                                         });

            return FuncHelper.Create(() =>
                                     {
                                         lock (locker)
                                         {
                                             var handle = waitHandle;

                                             if (handle != null)
                                             {
                                                 handle.WaitOne();
                                                 handle.Dispose();
                                             }
                                         }

                                         return result.GetValue(ex => new Exception("FactoryException", ex));
                                     });
        }
        else
        {
            var lazyValue = func.ToLazy();

            return () => lazyValue.Value;
        }
    }

    public static Func<TArg, TResult> WithCache<TArg, TResult>(this Func<TArg, TResult> func, IEqualityComparer<TArg> equalityComparer = null)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var cache = new Dictionary<TArg, TResult>(equalityComparer ?? EqualityComparer<TArg>.Default);

        return arg => cache.GetValueOrCreate(arg, () => func(arg));
    }

    public static Func<TResult> WithLock<TResult>(this Func<TResult> func, object baseLocker = null)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var locker = baseLocker ?? new object();

        return () =>
               {
                   lock (locker)
                   {
                       return func();
                   }
               };
    }

    public static Func<TArg, TResult> WithLock<TArg, TResult>(this Func<TArg, TResult> func, object baseLocker = null)
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

    public static Func<TArg1, TArg2, TResult> WithCache<TArg1, TArg2, TResult>(this Func<TArg1, TArg2, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        var cacheFunc = WithCache((Tuple<TArg1, TArg2> v) => func(v.Item1, v.Item2));

        return (arg1, arg2) => cacheFunc (Tuple.Create(arg1, arg2));
    }

    public static Func<TArg, IEnumerable<TResult>> Sum<TArg, TResult>(this IEnumerable<Func<TArg, IEnumerable<TResult>>> funcs)
    {
        if (funcs == null) throw new ArgumentNullException(nameof(funcs));

        return funcs.Aggregate(FuncHelper.Create((TArg _) => Enumerable.Empty<TResult>()), (f1, f2) => arg => f1(arg).Concat(f2(arg)));
    }

    public static Func<TArg1, TArg2, IEnumerable<TResult>> Sum<TArg1, TArg2, TResult>(this IEnumerable<Func<TArg1, TArg2, IEnumerable<TResult>>> funcs)
    {
        if (funcs == null) throw new ArgumentNullException(nameof(funcs));

        return funcs.Aggregate(FuncHelper.Create((TArg1 _, TArg2 __) => Enumerable.Empty<TResult>()), (f1, f2) => (arg1, arg2) => f1(arg1, arg2).Concat(f2(arg1, arg2)));
    }
}
