using System.Reflection;

using Anch.SecuritySystem.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class ProjectionDomainSecurityBssFrameworkExtension(Assembly assembly) : ISecuritySystemExtension
{
    public void AddServices(IServiceCollection services) => services.AddProjectionDomainSecurityServices(assembly);
}

