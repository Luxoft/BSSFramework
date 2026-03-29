using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL;

public partial class ConfigurationBLLFactoryContainer
{
    public ISubscriptionBLL Subscription => ServiceProviderServiceExtensions.GetRequiredService<ISubscriptionBLL>(this.Context.ServiceProvider);
}
