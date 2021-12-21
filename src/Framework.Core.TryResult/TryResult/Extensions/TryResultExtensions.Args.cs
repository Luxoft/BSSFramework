using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    public static partial class TryResultExtensions
    {
        public static void Match<TArgs, TResult>(this ITryResult<TArgs, TResult> source, Action<TArgs, TResult> successAction, Action<TArgs, Exception> faultAction = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (successAction == null) throw new ArgumentNullException(nameof(successAction));


            if (source is ISuccessResult<TArgs, TResult>)
            {
                var successedResult = (source as ISuccessResult<TArgs, TResult>);

                successAction(successedResult.Args, successedResult.Result);
            }
            else if (source is IFaultResult<TArgs, TResult>)
            {
                if (faultAction != null)
                {
                    var faultResult = source as IFaultResult<TArgs, TResult>;
                    faultAction(faultResult.Args, faultResult.Error);
                }
            }
            else
            {
                throw new Exception("Unknown tryResult type");
            }
        }

        public static TNext Match<TArgs, TResult, TNext>(
            this ITryResult<TArgs, TResult> source,
            Func<TArgs, TResult, TNext> successAction,
            Func<TArgs, Exception, TNext> getFaultResult)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (successAction == null) throw new ArgumentNullException(nameof(successAction));
            if (getFaultResult == null) throw new ArgumentNullException(nameof(getFaultResult));

            if (source is ISuccessResult<TArgs, TResult>)
            {
                var successResult = (source as ISuccessResult<TArgs, TResult>);

                return successAction(successResult.Args, successResult.Result);
            }
            else if (source is IFaultResult<TArgs, TResult>)
            {
                var faultResult = source as IFaultResult<TArgs, TResult>;
                return getFaultResult(faultResult.Args, faultResult.Error);
            }
            else
            {
                throw new Exception("Unknown tryResult type");
            }
        }

        public static void SuccessProgress<TArgs, TResult>(this ITryResult<TArgs, TResult> source, Action<TArgs, TResult> successAction)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (successAction == null) throw new ArgumentNullException(nameof(successAction));


            source.Match(successAction, (arg, error) => { throw error; });
        }

        public static bool IsSuccess<TArgs, TResult>(this ITryResult<TArgs, TResult> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source is ISuccessResult<TArgs, TResult>;
        }

        public static ISuccessResult<TArgs, TResult> ToSuccess<TArgs, TResult>(this ITryResult<TArgs, TResult> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return (ISuccessResult<TArgs, TResult>)source;
        }

        public static IFaultResult<TArgs, TResult> ToFault<TArgs, TResult>(this ITryResult<TArgs, TResult> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return (IFaultResult<TArgs, TResult>)source;
        }

        public static bool IsFault<TArgs, TResult>(this ITryResult<TArgs, TResult> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source is IFaultResult<TArgs, TResult>;
        }

        public static IEnumerable<Exception> GetErrors<TArgs, TResult>(this IEnumerable<ITryResult<TArgs, TResult>> source)
        {
            return source.OfType<IFaultResult<TArgs, TResult>>().Select(z => z.Error);
        }

        public static IEnumerable<TResult> GetResults<TArgs, TResult>(this IEnumerable<ITryResult<TArgs, TResult>> source)
        {
            return source.OfType<ISuccessResult<TArgs, TResult>>().Select(z => z.Result);
        }
    }
}