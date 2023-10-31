namespace Automation;

public class ServiceProviderPoolFunc
{
    public readonly Func<IServiceProvider> Value;

    public ServiceProviderPoolFunc(Func<IServiceProvider> func) => this.Value = func;
}
