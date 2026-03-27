namespace Framework.BLL.Domain.Exceptions;

public static class BusinessLogicExceptionExtensions
{
    public static BusinessLogicAggregateException Aggregate(this IEnumerable<BusinessLogicException> source) => new([..source]);

    public static BusinessLogicAggregateException Aggregate(this IEnumerable<Exception> exceptions) =>
        exceptions.Select(ex => ex as BusinessLogicException ?? new BusinessLogicException(ex.Message, ex)).Aggregate();
}
