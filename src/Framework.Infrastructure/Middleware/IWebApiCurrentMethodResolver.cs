using System.Reflection;

namespace Framework.Infrastructure.Middleware;

public interface IWebApiCurrentMethodResolver
{
    MethodInfo? TryGetCurrentMethod();
}
