using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkExtension(Action<IServiceCollection> addServicesAction) : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) => addServicesAction(services);
}
