using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;

namespace Framework.BLL.Domain.Exceptions.BusinessLogic.Extensions;

public static class BusinessLogicExceptionExtensions
{
    public static BusinessLogicAggregateException Aggregate (this IEnumerable<BusinessLogicException> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        return new BusinessLogicAggregateException(source);
    }

    public static BusinessLogicAggregateException Aggregate(this IEnumerable<Exception> exceptions)
    {
        return exceptions.Select(ex => ex as BusinessLogicException ?? new BusinessLogicException(ex, ex.Message)).Aggregate();
    }
}
