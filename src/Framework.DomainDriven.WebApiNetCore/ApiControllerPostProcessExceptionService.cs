using System;

using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.Exceptions;
using Framework.Validation;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerPostProcessExceptionService : IApiControllerPostProcessExceptionService
{
    public Exception Process(EvaluateException evaluateException, IConfigurationBLLContext context)
    {
        return this.GetFacadeException(evaluateException.ExpandedBaseException, context);
    }


    /// <summary>
    ///     Get Internal Server Exception
    /// </summary>
    protected virtual Exception GetInternalServerException() =>
            new Exception(InternalServerException.DefaultMessage);

    private Exception GetFacadeException(Exception exception, IConfigurationBLLContext context)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception));
        }

        return this.IsHandledException(exception) || context.DisplayInternalError
                       ? exception
                       : this.GetInternalServerException();
    }


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
