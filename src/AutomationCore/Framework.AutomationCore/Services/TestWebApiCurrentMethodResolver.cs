using System.Reflection;

using Framework.Infrastructure.Middleware;

namespace Framework.AutomationCore.Services;

public class TestWebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private MethodInfo? currentMethod;

    public MethodInfo? TryGetCurrentMethod() => this.currentMethod;

    public void SetCurrentMethod(MethodInfo methodInfo) => this.currentMethod = methodInfo;
}
