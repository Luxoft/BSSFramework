using CommonFramework;

using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;

namespace Framework.BLL.Domain.Exceptions.BusinessLogic;

public class BusinessLogicAggregateException : BusinessLogicException, IAggregateException<BusinessLogicException>
{
    public BusinessLogicAggregateException(IEnumerable<BusinessLogicException> innerExceptions)
            : this(innerExceptions.ToArray())
    {

    }

    public BusinessLogicAggregateException(BusinessLogicException[] innerExceptions)
            : base (innerExceptions.Join(Environment.NewLine, ex => ex.Message)) =>
        this.InnerExceptions = innerExceptions.ToReadOnlyCollection();

    public System.Collections.ObjectModel.ReadOnlyCollection<BusinessLogicException> InnerExceptions
    {
        get; private set;
    }
}
