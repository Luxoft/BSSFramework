namespace Automation;

public interface IServiceProviderPool
{
    IServiceProvider Get();
    void Release(IServiceProvider serviceProvider);
}