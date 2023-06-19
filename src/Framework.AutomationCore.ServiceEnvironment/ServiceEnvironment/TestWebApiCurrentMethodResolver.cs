using System.Reflection;

using Framework.DomainDriven.WebApiNetCore;

using Microsoft.SqlServer.Management.Dmf;

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
        if (this.currentMethod != null)
        {
            throw new InvalidInOperatorException();
        }

        this.currentMethod = methodInfo;
    }

    public void ClearCurrentMethod()
    {
        if (this.currentMethod == null)
        {
            throw new InvalidInOperatorException();
        }

        this.currentMethod = null;
    }
}
