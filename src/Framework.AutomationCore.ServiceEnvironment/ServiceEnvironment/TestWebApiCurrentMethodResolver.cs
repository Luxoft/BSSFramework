using System.Reflection;

using Framework.DomainDriven.WebApiNetCore;

namespace Automation.ServiceEnvironment;

public class TestWebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private MethodInfo currentMethod;

    public MethodInfo GetCurrentMethod()
    {
        return this.currentMethod;
    }

    public void SetCurrentMethod(MethodInfo methodInfo)
    {
        this.currentMethod = methodInfo;
    }
}
