using System;

namespace Framework.Core;

public static class CondionalExtension
{
    public static Condition<T> If<T>(this T source, Func<T, bool> conditionalFunc)
    {
        return new Condition<T>(source, conditionalFunc(source));
    }
    public static Condition<T> If<T>(this Condition<T> source, Func<T, bool> conditionalFunc)
    {
        if (!source.ConditionResult)
        {
            return source;
        }
        return new Condition<T>(source.Source, conditionalFunc(source.Source));
    }

    public static TResult Return<TSource, TResult>(this Condition<TSource> source, Func<TResult> truReturnFunc)
    {
        return source.Return(truReturnFunc());
    }

    public static TResult Return<TSource, TResult>(this Condition<TSource> source, TResult truReturnFunc)
    {
        return source.ConditionResult ? truReturnFunc : default(TResult);
    }

    public static void Do<TSource>(this Condition<TSource> source, Action doAction)
    {
        if (source.ConditionResult)
        {
            doAction();
        }
    }

    public static TResult Return<TSource, TResult>(this Condition<TSource> source, Func<TSource, TResult> truReturnFunc)
    {
        return source.Return(truReturnFunc(source.Source));
    }
}
