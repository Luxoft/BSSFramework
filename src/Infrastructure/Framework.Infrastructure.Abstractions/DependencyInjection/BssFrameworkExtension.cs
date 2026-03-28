using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class BssFrameworkExtension(Action<IServiceCollection> setupAction) : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
