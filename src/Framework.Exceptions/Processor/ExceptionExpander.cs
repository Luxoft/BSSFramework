using System.Reflection;

using Framework.Core;

namespace Framework.Exceptions;
public class ExceptionExpander : IExceptionExpander
{
    private static readonly MethodInfo ProcessAggregateExceptionMethod = typeof(ExceptionExpander).GetMethod(nameof(ProcessAggregateException), BindingFlags.NonPublic | BindingFlags.Instance, true);


    protected virtual Exception ProcessAggregateException<TCurrentException, TInnerException>(TCurrentException currentException)
            where TCurrentException : Exception, IAggregateException<TInnerException>
            where TInnerException : Exception
    {
        var innerExceptions = currentException.InnerExceptions.Select(this.Process).ToList();

        if (innerExceptions.Count == 1)
        {
            return innerExceptions[0];
        }
        else
        {
            return new WrappedAggregateException(currentException, innerExceptions);
        }
    }

    public virtual Exception Process(Exception exception)
    {
        var targetInvocationRequest = from targetInvocationException in (exception as TargetInvocationException).ToMaybe()
                                      let lastInnerException = targetInvocationException.GetLastInnerException()
                                      select this.Process(lastInnerException);

        return targetInvocationRequest
               .Or(() => from innerExceptionType in exception.GetType().GetAggregateExceptionInnerExceptionType().ToMaybe()
                         select (Exception)ProcessAggregateExceptionMethod.MakeGenericMethod(exception.GetType(), innerExceptionType).Invoke(this,
                             [exception]))
               .GetValueOrDefault(exception);
    }

    protected class WrappedAggregateException : BusinessLogicException
    {
        private readonly Exception baseAggregateException;

        public WrappedAggregateException(Exception baseAggregateException, IEnumerable<Exception> innerExceptions, string lineSeparator = null)
                : base(baseAggregateException, innerExceptions.Join(lineSeparator ?? Environment.NewLine, exception => exception.Message))
        {
            this.baseAggregateException = baseAggregateException ?? throw new ArgumentNullException(nameof(baseAggregateException));
        }

        public override string StackTrace => this.baseAggregateException.StackTrace;
    }
}
