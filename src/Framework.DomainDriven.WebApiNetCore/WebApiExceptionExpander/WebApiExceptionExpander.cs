using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.Exceptions;
using Framework.Notification;
using Framework.Validation;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiExceptionExpander : IWebApiExceptionExpander
{
    private readonly IContextEvaluator<IConfigurationBLLContext> contextEvaluator;

    private readonly IExceptionExpander exceptionExpander;

    public WebApiExceptionExpander(IContextEvaluator<IConfigurationBLLContext> contextEvaluator, IExceptionExpander exceptionExpander)
    {
        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
        this.exceptionExpander = exceptionExpander ?? throw new ArgumentNullException(nameof(exceptionExpander));
    }

    public Exception Process(Exception baseException)
    {
        var exception = this.exceptionExpander.Process(baseException);

        return this.IsHandledException(exception) || this.contextEvaluator.Evaluate(DBSessionMode.Read, context => context.DisplayInternalError)
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
                                         typeof(StaleDomainObjectStateException)
                                 };

        return exceptionType.IsAssignableToAny(expectedExceptions);
    }
}
