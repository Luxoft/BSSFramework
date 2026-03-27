using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.Setup;

public interface INHibernateSetupObjectExtension
{
    public void AddServices(IServiceCollection services);
}
