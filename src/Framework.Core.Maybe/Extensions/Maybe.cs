namespace Framework.Core;

public static class Maybe
{
    public static Maybe<T> ToMaybe<T>(this T value)
            where T : class
    {
        return OfCondition(value != null, () => value);
    }

    public static Maybe<T> ToMaybe<T>(this T? value)
            where T : struct
    {
        return OfCondition(value != null, () => value.Value);
    }

    public static Maybe<string> ToMaybe(this string value)
    {
        return OfCondition(!string.IsNullOrWhiteSpace(value), () => value);
    }

    [Obsolete("v10 for delete")]
    public static Maybe<T> AsMaybe<T>(this object value)
            where T : class
    {
        return (value as T).ToMaybe();
    }

    [Obsolete("v10 for delete")]
    public static Maybe<TTarget> AsMaybeStrong<TTarget, TSource>(this TSource value)
            where TTarget : class, TSource
            where TSource : class
    {
        return value.AsMaybe<TTarget>();
    }


    public static Maybe<T> OfCondition<T>(bool condition, Func<T> getJustValue)
    {
        if (getJustValue == null) throw new ArgumentNullException(nameof(getJustValue));

        return OfCondition(condition, () => Maybe.Return(getJustValue()), () => Maybe<T>.Nothing);
    }


    public static Maybe<T> OfCondition<T>(bool condition, Func<Maybe<T>> getTrueValue, Func<Maybe<T>> getFalseValue)
    {
        if (getTrueValue == null) throw new ArgumentNullException(nameof(getTrueValue));
        if (getFalseValue == null) throw new ArgumentNullException(nameof(getFalseValue));

        return condition ? getTrueValue()
                       : getFalseValue();
    }

    public static Maybe<T> Return<T> (T value)
    {
        return new Just<T>(value);
    }

    public static Maybe<T?> ReturnNullable<T>(T? value)
            where T : struct
    {
        return Return(value);
    }


    public static Maybe<Ignore> Return()
    {
        return Return(Ignore.Value);
    }

    public static Func<TArg, Maybe<TResult>> OfTryMethod<TArg, TResult>(TryMethod<TArg, TResult> tryAction)
    {
        if (tryAction == null) throw new ArgumentNullException(nameof(tryAction));

        return arg =>
               {
                   TResult result;
                   return OfCondition(tryAction(arg, out result), () => result);
               };
    }

    public static Func<TArg1, TArg2, Maybe<TResult>> OfTryMethod<TArg1, TArg2, TResult>(TryMethod<TArg1, TArg2, TResult> tryAction)
    {
        if (tryAction == null) throw new ArgumentNullException(nameof(tryAction));

        return (arg1, arg2) =>
               {
                   TResult result;
                   return OfCondition(tryAction(arg1, arg2, out result), () => result);
               };
    }


}
