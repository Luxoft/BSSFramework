namespace Framework.AutomationCore.ServiceProviderPool;

public interface IServiceProviderPool
{
    IServiceProvider Get();
    void Release(IServiceProvider serviceProvider);
}
