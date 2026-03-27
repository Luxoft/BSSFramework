using System.Reflection;

using CommonFramework;
using CommonFramework.Maybe;

using Framework.BLL.Domain.Exceptions;
using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;
using Framework.BLL.Domain.Exceptions.Extensions;
using Framework.Core;

namespace Framework.BLL.Services;

public class ExceptionExpander : IExceptionExpander
{
    private static readonly MethodInfo ProcessAggregateExceptionMethod = typeof(ExceptionExpander).GetMethod(nameof(ProcessAggregateException), BindingFlags.NonPublic | BindingFlags.Instance, true)!;


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
                                      let lastInnerException = targetInvocationException.GetBaseException()
                                      select this.Process(lastInnerException);

        return targetInvocationRequest
               .Or(() => from innerExceptionType in exception.GetType().GetAggregateExceptionInnerExceptionType().ToMaybe()

                         select ProcessAggregateExceptionMethod.MakeGenericMethod(exception.GetType(), innerExceptionType).Invoke<Exception>(
                             this,
                             [exception]))

               .GetValueOrDefault(exception);
    }

    protected class WrappedAggregateException(Exception baseAggregateException, IEnumerable<Exception> innerExceptions, string? lineSeparator = null)
        : BusinessLogicException(baseAggregateException, innerExceptions.Join(lineSeparator ?? Environment.NewLine, exception => exception.Message))
    {
        private readonly Exception baseAggregateException = baseAggregateException ?? throw new ArgumentNullException(nameof(baseAggregateException));

        public override string? StackTrace => this.baseAggregateException.StackTrace;
    }
}
