using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public class EntityFrameworkSetupExtension(Action<IServiceCollection> setupAction) : IEntityFrameworkSetupExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
