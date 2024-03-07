using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public interface IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services);
}
