using System.Reflection;

using Framework.DomainDriven.Setup;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class ProjectionDomainSecurityBssFrameworkExtension : IBssFrameworkExtension
{
    private readonly Assembly assembly;

    public ProjectionDomainSecurityBssFrameworkExtension(Assembly assembly)
    {
        this.assembly = assembly;
    }

    public void AddServices(IServiceCollection services) => services.RegisterProjectionDomainSecurityServices(this.assembly);
}
