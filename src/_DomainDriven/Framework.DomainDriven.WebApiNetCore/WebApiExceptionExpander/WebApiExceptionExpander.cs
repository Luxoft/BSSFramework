using Framework.Core;
using Framework.DomainDriven.DALExceptions;
using Framework.Exceptions;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Bss;
using Framework.Validation;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiExceptionExpander : IWebApiExceptionExpander
{
    private readonly IAuthorizationSystem authorizationSystem;

    private readonly IExceptionExpander exceptionExpander;

    public WebApiExceptionExpander(IAuthorizationSystem authorizationSystem, IExceptionExpander exceptionExpander)
    {
        this.authorizationSystem = authorizationSystem ?? throw new ArgumentNullException(nameof(authorizationSystem));
        this.exceptionExpander = exceptionExpander ?? throw new ArgumentNullException(nameof(exceptionExpander));
    }

    public Exception Process(Exception baseException)
    {
        var exception = this.exceptionExpander.Process(baseException);

        return this.IsHandledException(exception) || this.authorizationSystem.HasAccess(BssSecurityOperation.DisplayInternalError)
                       ? exception
                       : this.GetInternalServerException();
    }

    /// <summary>
    ///     Get Internal Server Exception
    /// </summary>
    protected virtual Exception GetInternalServerException() => new(InternalServerException.DefaultMessage);

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
                                         typeof(IntergationException),
                                         typeof(SecurityException),
                                         typeof(ValidationException),
                                         typeof(DALException),
                                         typeof(StaleDomainObjectStateException),
                                         typeof(AccessDeniedException)
                                 };

        return exceptionType.IsAssignableToAny(expectedExceptions);
    }
}
