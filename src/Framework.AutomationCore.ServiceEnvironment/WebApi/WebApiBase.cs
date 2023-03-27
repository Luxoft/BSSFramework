namespace Automation.ServiceEnvironment;

public abstract class WebApiBase : RootServiceProviderContainer
{
    protected WebApiBase(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }
}
