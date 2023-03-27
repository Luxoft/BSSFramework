namespace Framework.Core;

public static class MaybeExtensions
{
    public static Maybe<TResult> Select<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return source.Match(result => Maybe.Return(selector(result)), () => Maybe<TResult>.Nothing);
    }


    public static Maybe<TResult> SelectMany<TSource, TNextResult, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TNextResult>> nextSelector, Func<TSource, TNextResult, TResult> resultSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (nextSelector == null) throw new ArgumentNullException(nameof(nextSelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return source.Match(result1 => nextSelector(result1).Match(result2 => Maybe.Return(resultSelector(result1, result2)),
                                                                   ()      => Maybe<TResult>.Nothing),
                            ()      => Maybe<TResult>.Nothing);
    }

    public static Maybe<TResult> SelectMany<TSource, TResult>(this Maybe<TSource> source, Func<TSource, Maybe<TResult>> nextSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (nextSelector == null) throw new ArgumentNullException(nameof(nextSelector));

        return source.SelectMany(nextSelector, (_, res) => res);
    }



    public static Maybe<T> Where<T>(this Maybe<T> source, Func<T, bool> filter)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return source.Match(result => Maybe.OfCondition(filter(result), () => result), () => Maybe<T>.Nothing);
    }

    public static Maybe<TResult> Zip<TSource1, TSource2, TResult>(this Maybe<TSource1> source1, Maybe<TSource2> source2, Func<TSource1, TSource2, TResult> resultSelector)
    {
        if (source1 == null) throw new ArgumentNullException(nameof(source1));
        if (source2 == null) throw new ArgumentNullException(nameof(source2));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return from v1 in source1

               from v2 in source2

               select resultSelector(v1, v2);
    }

    public static Maybe<TResult> Join<TOuter, TInner, TKey, TResult>(this Maybe<TOuter> outer, Maybe<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
    {
        if (outer == null) throw new ArgumentNullException(nameof(outer));
        if (inner == null) throw new ArgumentNullException(nameof(inner));
        if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
        if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        return outer.Join(inner, outerKeySelector, innerKeySelector, resultSelector, null);
    }


    public static Maybe<TResult> Join<TOuter, TInner, TKey, TResult>(this Maybe<TOuter> outer, Maybe<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> preComparer)
    {
        if (outer == null) throw new ArgumentNullException(nameof(outer));
        if (inner == null) throw new ArgumentNullException(nameof(inner));
        if (outerKeySelector == null) throw new ArgumentNullException(nameof(outerKeySelector));
        if (innerKeySelector == null) throw new ArgumentNullException(nameof(innerKeySelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

        var comparer = preComparer ?? EqualityComparer<TKey>.Default;

        return from v1 in outer

               from v2 in inner

               let k1 = outerKeySelector(v1)

               let k2 = innerKeySelector(v2)

               where comparer.GetHashCode(k1) == comparer.GetHashCode(k2) && comparer.Equals(k1, k2)

               select resultSelector(v1, v2);
    }




    public static void MaybeJustValue<T> (this Maybe<T> maybeValue, Action<T> action)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (action == null) throw new ArgumentNullException(nameof(action));

        (maybeValue as Just<T>).Maybe(just => action(just.Value));
    }

    public static TResult MaybeJustValue<TSource, TResult>(this Maybe<TSource> maybeValue, Func<TSource, TResult> action)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (action == null) throw new ArgumentNullException(nameof(action));

        return (maybeValue as Just<TSource>).Maybe(just => action(just.Value));
    }

    public static void Match<TSource>(this Maybe<TSource> maybeValue, Action<TSource> fromJustAction, Action fromNothingAction = null)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (fromJustAction == null) throw new ArgumentNullException(nameof(fromJustAction));

        var just = maybeValue as Just<TSource>;

        if (just == null)
        {
            fromNothingAction?.Invoke();
        }
        else
        {
            fromJustAction(just.Value);
        }
    }

    public static TResult Match<TSource, TResult>(this Maybe<TSource> maybeValue, Func<TSource, TResult> fromJustResult, Func<TResult> fromNothingResult)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (fromJustResult == null) throw new ArgumentNullException(nameof(fromJustResult));
        if (fromNothingResult == null) throw new ArgumentNullException(nameof(fromNothingResult));

        var just = maybeValue as Just<TSource>;

        return just == null ? fromNothingResult() : fromJustResult(just.Value);
    }


    public static T GetValue<T>(this Maybe<T> maybeValue)
    {
        return maybeValue.GetValue(() => new Exception("Nothing Value"));
    }

    public static T GetValue<T>(this Maybe<T> maybeValue, Func<Exception> nothingException)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (nothingException == null) throw new ArgumentNullException(nameof(nothingException));

        return maybeValue.Match(result => result, () => { throw nothingException(); });
    }

    public static T GetValue<T>(this Maybe<T> maybeValue, Func<string> nothingExceptionText)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (nothingExceptionText == null) throw new ArgumentNullException(nameof(nothingExceptionText));

        return maybeValue.GetValue(() => new Exception(nothingExceptionText()));
    }

    public static T GetValue<T>(this Maybe<T> maybeValue, string nothingExceptionText)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (nothingExceptionText == null) throw new ArgumentNullException(nameof(nothingExceptionText));

        return maybeValue.GetValue(() => nothingExceptionText);
    }

    public static T GetValueOrDefault<T>(this Maybe<T> maybeValue)
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));

        return maybeValue.GetValueOrDefault(default(T));
    }


    public static TResult GetValueOrDefault<TSource, TResult>(this Maybe<TSource> maybeValue, TResult defaultValue)
            where TSource : TResult
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));

        return maybeValue.Select(v => (TResult)v).GetValueOrDefault(() => defaultValue);
    }

    public static TResult GetValueOrDefault<TSource, TResult>(this Maybe<TSource> maybeValue, Func<TResult> getDefaultValue)
            where TSource : TResult
    {
        if (maybeValue == null) throw new ArgumentNullException(nameof(maybeValue));
        if (getDefaultValue == null) throw new ArgumentNullException(nameof(getDefaultValue));

        return maybeValue.Match(result => result, getDefaultValue);
    }

    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, Maybe<TResult> v2)
            where TSource : TResult
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (v2 == null) throw new ArgumentNullException(nameof(v2));

        return v1.HasValue ? v1.Select(v => (TResult)v) : v2;
    }

    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, Func<TResult> getV2)
            where TSource : TResult
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (getV2 == null) throw new ArgumentNullException(nameof(getV2));

        return v1.HasValue ? v1.Select(v => (TResult)v) : Maybe.Return(getV2());
    }

    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, TResult v2)
            where TSource : TResult
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));

        return v1.HasValue ? v1.Select(v => (TResult)v) : Maybe.Return(v2);
    }

    public static Maybe<TResult> Or<TSource, TResult>(this Maybe<TSource> v1, Func<Maybe<TResult>> getV2)
            where TSource : TResult
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));

        return v1.HasValue ? v1.Select(v => (TResult)v) : getV2();
    }


    public static T ToReference<T>(this Maybe<T> value)
            where T : class
    {
        return value.Match(v => v, () => default(T));
    }

    public static T? ToNullable<T>(this Maybe<T> value)
            where T : struct
    {
        return value.Match(v => v, () => default(T?));
    }


    private static readonly Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>> LogicOrDict = new Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>>()
        {
                //{ Maybe.Return(true), new Dictionary<Maybe<bool>, Maybe<bool>>
                //                      {
                //                          { Maybe.Return(true), Maybe.Return(true) },
                //                          { Maybe.Return(false), Maybe.Return(true) },
                //                          { Maybe<bool>.Nothing, Maybe.Return(true) }
                //                      } },

                { Maybe.Return(false), new Dictionary<Maybe<bool>, Maybe<bool>>
                                       {
                                               { Maybe.Return(true), Maybe.Return(true) },
                                               { Maybe.Return(false), Maybe.Return(false) },
                                               { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
                                       } },

                {  Maybe<bool>.Nothing, new Dictionary<Maybe<bool>, Maybe<bool>>
                                        {
                                                { Maybe.Return(true), Maybe.Return(true) },
                                                { Maybe.Return(false), Maybe<bool>.Nothing },
                                                { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
                                        } },
        };


    public static Maybe<bool> LogicOr(this Maybe<bool> v1, Func<Maybe<bool>> getV2)
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (getV2 == null) throw new ArgumentNullException(nameof(getV2));

        return v1.Where(p => p).Or(() => LogicOrDict[v1][getV2()]);
    }

    public static Maybe<bool> LogicOr(this Maybe<bool> v1, Maybe<bool> v2)
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (v2 == null) throw new ArgumentNullException(nameof(v2));

        return v1.LogicOr(() => v2);
    }




    private static readonly Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>> LogicAndDict = new Dictionary<Maybe<bool>, Dictionary<Maybe<bool>, Maybe<bool>>>()
        {
                { Maybe.Return(true), new Dictionary<Maybe<bool>, Maybe<bool>>
                                      {
                                              { Maybe.Return(true), Maybe.Return(true) },
                                              { Maybe.Return(false), Maybe.Return(false) },
                                              { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
                                      } },

                //{ Maybe.Return(false), new Dictionary<Maybe<bool>, Maybe<bool>>
                //                       {
                //                           { Maybe.Return(true), Maybe.Return(false) },
                //                           { Maybe.Return(false), Maybe.Return(false) },
                //                           { Maybe<bool>.Nothing, Maybe.Return(false) }
                //                       } },

                {  Maybe<bool>.Nothing, new Dictionary<Maybe<bool>, Maybe<bool>>
                                        {
                                                { Maybe.Return(true), Maybe<bool>.Nothing },
                                                { Maybe.Return(false), Maybe.Return(false) },
                                                { Maybe<bool>.Nothing, Maybe<bool>.Nothing }
                                        } },
        };



    public static Maybe<bool> LogicAnd(this Maybe<bool> v1, Func<Maybe<bool>> getV2)
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (getV2 == null) throw new ArgumentNullException(nameof(getV2));

        return v1.Where(p => !p).Or(() => LogicAndDict[v1][getV2()]);
    }

    public static Maybe<bool> LogicAnd(this Maybe<bool> v1, Maybe<bool> v2)
    {
        if (v1 == null) throw new ArgumentNullException(nameof(v1));
        if (v2 == null) throw new ArgumentNullException(nameof(v2));

        return v1.LogicAnd(() => v2);
    }
}


public delegate bool TryMethod<in TArg, TResult>(TArg arg, out TResult result);

public delegate bool TryMethod<in TArg1, in TArg2, TResult>(TArg1 arg1, TArg2 arg2, out TResult result);
