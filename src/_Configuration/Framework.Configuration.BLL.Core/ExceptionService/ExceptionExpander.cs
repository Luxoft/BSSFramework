using System.Reflection;

using Framework.Core;
using Framework.Exceptions;
using Framework.Notification;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public class ExceptionExpander : IExceptionExpander
{
    private readonly MethodInfo processAggregateExceptionMethod;

    public ExceptionExpander()
    {
        this.processAggregateExceptionMethod = new Func<AggregateValidationException, Exception>(this.ProcessAggregateException<AggregateValidationException, ValidationExceptionBase>).Method.GetGenericMethodDefinition();
    }

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
        var convertibleExceptionRequest = from convertible in (exception as IConvertible<ValidationException>).ToMaybe()
                                          let next = convertible.Convert()
                                          select this.Process(next);

        var targetInvocationRequest = from targetInvocationException in (exception as TargetInvocationException).ToMaybe()
                                      let lastInnerException = targetInvocationException.GetLastInnerException()
                                      select this.Process(lastInnerException);

        return convertibleExceptionRequest
               .Or(targetInvocationRequest)
               .Or(
                   () => from innerExceptionType in exception.GetType().GetAggregateExceptionInnerExceptionType().ToMaybe()
                         select (Exception)this.processAggregateExceptionMethod.MakeGenericMethod(exception.GetType(), innerExceptionType).Invoke(this, new[] { exception }))
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
