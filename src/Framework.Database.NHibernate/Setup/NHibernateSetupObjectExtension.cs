using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.Setup;

public class NHibernateSetupObjectExtension(Action<IServiceCollection> setupAction) : INHibernateSetupObjectExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
