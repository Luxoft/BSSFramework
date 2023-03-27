namespace Framework.Core;

public static partial class TryResultExtensions
{
    public static ITryResult<TResult> Select<TSource, TResult>(this ITryResult<TSource> source, Func<TSource, TResult> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));


        return source.Match(result => TryResult.Return(selector(result)), TryResult.CreateFault<TResult>);
    }


    public static ITryResult<TResult> SelectMany<TSource, TNextSource, TResult>(this ITryResult<TSource> source, Func<TSource, ITryResult<TNextSource>> nextSelector, Func<TSource, TNextSource, TResult> resultSelector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (nextSelector == null) throw new ArgumentNullException(nameof(nextSelector));
        if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));


        return source.Match(result => nextSelector(result).Match(nextResult => TryResult.Return(resultSelector(result, nextResult)),
                                                                 TryResult.CreateFault<TResult>),
                            TryResult.CreateFault<TResult>);
    }


    public static ITryResult<TResult> SelectMany<TSource, TResult>(this ITryResult<TSource> source, Func<TSource, ITryResult<TResult>> selector)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (selector == null) throw new ArgumentNullException(nameof(selector));


        return source.SelectMany(selector, (_, res) => res);
    }

    public static ITryResult<T> SelectMany<T>(this ITryResult<ITryResult<T>> value)
    {
        return value.Match(res => res, TryResult.CreateFault<T>);
    }


    public static void Match<T>(this ITryResult<T> source, Action<T> successAction, Action<Exception> faultAction = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (successAction == null) throw new ArgumentNullException(nameof(successAction));


        switch (source)
        {
            case ISuccessResult<T> result:
                successAction(result.Result);
                break;

            case IFaultResult<T> faultResult:
                faultAction?.Invoke(faultResult.Error);
                break;

            default:
                throw new Exception("Unknown tryResult type");
        }
    }


    public static TResult Match<T, TResult>(this ITryResult<T> source, Func<T, TResult> successAction, Func<Exception, TResult> getFaultResult)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (successAction == null) throw new ArgumentNullException(nameof(successAction));
        if (getFaultResult == null) throw new ArgumentNullException(nameof(getFaultResult));

        if (source is ISuccessResult<T>)
        {
            return successAction((source as ISuccessResult<T>).Result);
        }
        else if (source is IFaultResult<T>)
        {
            return getFaultResult((source as IFaultResult<T>).Error);
        }
        else
        {
            throw new Exception("Unknown tryResult type");
        }
    }


    public static void SuccessProgress<T>(this ITryResult<T> source, Action<T> successAction)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (successAction == null) throw new ArgumentNullException(nameof(successAction));


        source.Match(successAction, error => { throw error; });
    }


    public static TResult SuccessProgress<T, TResult>(this ITryResult<T> source, Func<T, TResult> successFunc)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (successFunc == null) throw new ArgumentNullException(nameof(successFunc));


        return source.Match(successFunc, error => { throw error; });
    }


    public static void SuccessProgressOrSkipBreak<T>(this ITryResult<T> source, Action<T> successAction)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (successAction == null) throw new ArgumentNullException(nameof(successAction));


        source.Match(successAction, _ => source.Validate());
    }


    public static void Validate<T>(this ITryResult<T> source, bool skipBreak = true)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        source.Match(_ => { }, error =>
                               {
                                   if (!skipBreak || !source.IsBreak()) { throw error; }
                               });
    }


    public static bool IsSuccess<T>(this ITryResult<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source is ISuccessResult<T>;
    }

    public static bool IsFault<T>(this ITryResult<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source is IFaultResult<T>;
    }

    public static bool IsBreak<T>(this ITryResult<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Match(_ => false, error => error is BreakException);
    }


    public static T GetValue<T>(this ITryResult<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.GetValue(ex => ex);
    }

    public static T GetValue<T>(this ITryResult<T> source, Func<Exception, Exception> getThrowException)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Match(v => v, error => { throw getThrowException(error); });
    }

    public static T GetValueOrDefault<T>(this ITryResult<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Match(v => v, error => default(T));
    }

    public static ITryResult<T> ToTryResult<T>(this Maybe<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return source.Match(TryResult.Return<T>, TryResult.CreateBreak<T>);
    }

    public static IEnumerable<Exception> GetErrors<T>(this IEnumerable<ITryResult<T>> source)
    {
        return from tryRes in source

               select tryRes.Match(_ => default(Exception), ex => ex) into ex

               where ex != null

               select ex;

    }

    public static IEnumerable<T> GetResults<T>(this IEnumerable<ITryResult<T>> source)
    {
        return from tryRes in source

               select tryRes.Match(res => new { res }, _ => null) into resCont

               where resCont != null

               select resCont.res;

    }


    public static void TryFault<T>(this IEnumerable<ITryResult<T>> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        source.TryFault(exceptions => new AggregateException(exceptions));
    }

    public static void TryFault<T>(this IEnumerable<ITryResult<T>> source, Func<Exception[], Exception> getAggregateException)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (getAggregateException == null) throw new ArgumentNullException(nameof(getAggregateException));

        var errors = source.GetErrors().ToArray();

        if (errors.Any())
        {
            throw getAggregateException(errors);
        }
    }
}
