namespace Framework.AutomationCore.ServiceEnvironment.RootServiceProviderContainer;

public abstract class RootServiceProviderContainer<TBLLContext>(IServiceProvider rootServiceProvider)
    : RootServiceProviderContainer(rootServiceProvider), IRootServiceProviderContainer<TBLLContext>;
