using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.DependencyInjection;

public interface IEntityFrameworkSetupExtension
{
    public void AddServices(IServiceCollection services);
}
