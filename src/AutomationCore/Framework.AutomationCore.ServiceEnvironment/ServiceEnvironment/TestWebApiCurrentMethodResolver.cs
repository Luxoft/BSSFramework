using System.Reflection;

using Framework.Infrastructure.Middleware;

namespace Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;

public class TestWebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private MethodInfo? currentMethod;

    public MethodInfo? TryGetCurrentMethod()
    {
        return this.currentMethod;
    }

    public void SetCurrentMethod(MethodInfo methodInfo)
    {
        this.currentMethod = methodInfo;
    }
}
