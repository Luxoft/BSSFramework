using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class ProjectionDomainSecurityBssFrameworkExtension(Assembly assembly) : ISecuritySystemExtension
{
    public void AddServices(IServiceCollection services) => services.AddProjectionDomainSecurityServices(assembly);
}
