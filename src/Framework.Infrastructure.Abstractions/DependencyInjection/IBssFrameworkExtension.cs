using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public interface IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services);
}
