using CommonFramework;

namespace Framework.BLL.Domain.Exceptions;

public class BusinessLogicAggregateException(BusinessLogicException[] innerExceptions)
    : BusinessLogicException(innerExceptions.Join(Environment.NewLine, ex => ex.Message)), IAggregateException<BusinessLogicException>
{
    public System.Collections.ObjectModel.ReadOnlyCollection<BusinessLogicException> InnerExceptions
    {
        get;
    } = innerExceptions.ToReadOnlyCollection();
}
