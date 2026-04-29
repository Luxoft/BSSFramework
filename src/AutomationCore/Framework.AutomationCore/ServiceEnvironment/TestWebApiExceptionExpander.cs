using Framework.Core;
using Framework.Infrastructure.WebApiExceptionExpander;

namespace Framework.AutomationCore.ServiceEnvironment;

/// <inheritdoc />
public class TestWebApiExceptionExpander(IExceptionExpander exceptionExpander) : IWebApiExceptionExpander
{
    private readonly IExceptionExpander exceptionExpander = exceptionExpander ?? throw new ArgumentNullException(nameof(exceptionExpander));

    /// <inheritdoc />
    public Exception? TryExpand(Exception baseException) => this.exceptionExpander.TryExpand(baseException);
}
