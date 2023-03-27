using Framework.Core;

namespace Framework.Exceptions;

public class BusinessLogicAggregateException : BusinessLogicException, IAggregateException<BusinessLogicException>
{
    public BusinessLogicAggregateException(IEnumerable<BusinessLogicException> innerExceptions)
            : this(innerExceptions.ToArray())
    {

    }

    public BusinessLogicAggregateException(BusinessLogicException[] innerExceptions)
            : base (innerExceptions.Join(Environment.NewLine, ex => ex.Message))
    {
        this.InnerExceptions = innerExceptions.ToReadOnlyCollection();
    }


    public System.Collections.ObjectModel.ReadOnlyCollection<BusinessLogicException> InnerExceptions
    {
        get; private set;
    }
}
