using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public class NHibernateSetupObjectExtension(Action<IServiceCollection> setupAction) : INHibernateSetupObjectExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
