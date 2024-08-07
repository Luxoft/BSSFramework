using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkExtension(Action<IServiceCollection> setupAction) : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
