namespace Automation.ServiceEnvironment;

public abstract class RootServiceProviderContainer<TBLLContext> : RootServiceProviderContainer, IRootServiceProviderContainer<TBLLContext>
{
    protected RootServiceProviderContainer(IServiceProvider rootServiceProvider)
        : base(rootServiceProvider)
    {
    }
}
