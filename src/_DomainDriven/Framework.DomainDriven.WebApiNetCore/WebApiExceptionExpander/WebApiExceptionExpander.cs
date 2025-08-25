using Framework.Core;
using Framework.DomainDriven.DALExceptions;
using Framework.Exceptions;
using SecuritySystem;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiExceptionExpander(IExceptionExpander exceptionExpander) : IWebApiExceptionExpander
{
    public Exception Process(Exception baseException)
    {
        var exception = exceptionExpander.Process(baseException);

        return this.IsHandledException(exception)
                       ? exception
                       : this.GetInternalServerException(exception);
    }

    /// <summary>
    ///     Get Internal Server Exception
    /// </summary>
    protected virtual Exception GetInternalServerException(Exception exception) => new InternalServerException(InternalServerException.DefaultMessage, exception);

    /// <summary>
    ///     Is Handled Exception
    /// </summary>
    protected virtual bool IsHandledException(Exception exception)
    {
        if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

        var exceptionType = exception.GetType();

        var expectedExceptions = new[]
                                 {
                                         typeof(BusinessLogicException),
                                         typeof(IntegrationException),
                                         typeof(SecurityException),
                                         typeof(Validation.ValidationException),
                                         typeof(DALException),
                                         typeof(StaleDomainObjectStateException),
                                         typeof(SecuritySystemException),
                                         typeof(FluentValidation.ValidationException)

                                 };

        return exceptionType.IsAssignableToAny(expectedExceptions);
    }
}
