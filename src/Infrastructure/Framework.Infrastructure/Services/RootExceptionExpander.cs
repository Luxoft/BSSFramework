using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.Services;

public class RootExceptionExpander([FromKeyedServices(IExceptionExpander.ElementKey)] IEnumerable<IExceptionExpander> exceptionExpanders) : IExceptionExpander
{
    public Exception? TryExpand(Exception exception)
    {
        var result = exceptionExpanders.Aggregate(exception, (state, expander) => expander.Expand(state));

        return result == exception ? null : result;
    }
}
