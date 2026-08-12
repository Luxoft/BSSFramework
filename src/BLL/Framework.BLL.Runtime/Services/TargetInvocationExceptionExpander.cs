using System.Reflection;

using Framework.Core;

namespace Framework.BLL.Services;

public class TargetInvocationExceptionExpander : IExceptionExpander
{
    public Exception? TryExpand(Exception exception) =>
        exception is TargetInvocationException targetInvocationException ? targetInvocationException.GetBaseException() : null;
}
