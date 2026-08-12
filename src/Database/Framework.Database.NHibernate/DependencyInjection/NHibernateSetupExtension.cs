using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public class NHibernateSetupExtension(Action<IServiceCollection> setupAction) : INHibernateSetupExtension
{
    public void AddServices(IServiceCollection services) => setupAction(services);
}
