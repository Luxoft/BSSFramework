namespace Framework.Core;

public static class FuncHelper
{
    public static Func<TResult> Create<TResult>(Func<TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Func<T, TResult> Create<T, TResult>(Func<T, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Func<T1, T2, TResult> Create<T1, T2, TResult>(Func<T1, T2, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Func<T1, T2, T3, TResult> Create<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }

    public static Func<T1, T2, T3, T4, TResult> Create<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> func)
    {
        if (func == null) throw new ArgumentNullException(nameof(func));

        return func;
    }
}
