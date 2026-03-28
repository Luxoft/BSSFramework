using System.Collections.Concurrent;

using Framework.Application.Services;
using Framework.Core;

namespace Framework.Infrastructure.WebApiExceptionExpander;

public class WebApiExceptionExpander(WebApiExceptionExpanderSettings settings, IExceptionExpander exceptionExpander) : IWebApiExceptionExpander
{
    private readonly ConcurrentDictionary<Type, bool> isHandledExceptionCache = [];

    public Exception Process(Exception baseException)
    {
        var exception = exceptionExpander.Process(baseException);

        return this.IsHandledException(exception)
                   ? exception
                   : this.GetInternalServerException(exception);
    }

    /// <summary>
    /// Get Internal Server Exception
    /// </summary>
    protected virtual Exception GetInternalServerException(Exception exception) => new InternalServerException(InternalServerException.DefaultMessage, exception);

    /// <summary>
    /// Is Handled Exception
    /// </summary>
    protected virtual bool IsHandledException(Exception exception) => this.isHandledExceptionCache.GetOrAdd(
        exception.GetType(),
        exceptionType => exceptionType.IsAssignableToAny(settings.HandledTypes));
}
