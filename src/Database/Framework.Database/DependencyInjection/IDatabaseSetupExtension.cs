using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public interface IDatabaseSetupExtension
{
    void AddServices(IServiceCollection services);
}
