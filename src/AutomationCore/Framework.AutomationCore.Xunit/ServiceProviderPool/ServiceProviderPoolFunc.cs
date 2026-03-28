namespace Automation.Xunit.ServiceProviderPool;

public class ServiceProviderPoolFunc(Func<IServiceProvider> func)
{
    public readonly Func<IServiceProvider> Value = func;
}
