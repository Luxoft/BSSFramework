using CommonFramework;

using Framework.Application;

namespace Framework.BLL.Exceptions;

public static class BusinessLogicExceptionExtensions
{
    public static BusinessLogicException Aggregate(this IEnumerable<BusinessLogicException> source)
    {
        var innerExceptions = source.ToArray();

        if (innerExceptions.Length == 1)
        {
            return innerExceptions[0];
        }
        else
        {
            return new BusinessLogicException(innerExceptions.Join(Environment.NewLine, ex => ex.Message), new AggregateException(innerExceptions.Cast<Exception>()));
        }
    }

    public static BusinessLogicException Aggregate(this IEnumerable<Exception> exceptions) =>
        exceptions.Select(ex => ex as BusinessLogicException ?? new BusinessLogicException(ex.Message, ex)).Aggregate();
}
