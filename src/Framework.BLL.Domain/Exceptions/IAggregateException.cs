using System.Collections.ObjectModel;

namespace Framework.BLL.Domain.Exceptions;

public interface IAggregateException<TInnerException>
        where TInnerException : Exception
{
    ReadOnlyCollection<TInnerException> InnerExceptions { get; }
}
