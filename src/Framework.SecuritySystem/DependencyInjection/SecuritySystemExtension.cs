using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class SecuritySystemExtension(Action<IServiceCollection> addServicesAction) : ISecuritySystemExtension
{
    public void AddServices(IServiceCollection services) => addServicesAction(services);
}
