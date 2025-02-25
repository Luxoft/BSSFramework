using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObjectExtension
{
    public void AddServices(IServiceCollection services);
}
