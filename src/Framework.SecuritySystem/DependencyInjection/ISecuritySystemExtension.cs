using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public interface ISecuritySystemExtension
{
    public void AddServices(IServiceCollection services);
}
