using System.Collections.ObjectModel;

namespace Framework.Exceptions;

public interface IAggregateException<TInnerException>
        where TInnerException : Exception
{
    ReadOnlyCollection<TInnerException> InnerExceptions { get; }
}
