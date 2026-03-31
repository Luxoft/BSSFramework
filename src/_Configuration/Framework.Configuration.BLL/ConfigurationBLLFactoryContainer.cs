using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL;

public partial class ConfigurationBLLFactoryContainer
{
    public ISubscriptionBLL Subscription => this.Context.ServiceProvider.GetRequiredService<ISubscriptionBLL>();
}
