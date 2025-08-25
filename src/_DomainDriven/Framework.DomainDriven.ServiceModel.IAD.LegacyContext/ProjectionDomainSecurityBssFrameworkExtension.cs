using System.Reflection;

using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionDomainSecurityBssFrameworkExtension(Assembly assembly) : ISecuritySystemExtension
{
    public void AddServices(IServiceCollection services) => services.RegisterProjectionDomainSecurityServices(assembly);
}
