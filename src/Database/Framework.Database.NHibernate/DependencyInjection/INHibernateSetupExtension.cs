using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.NHibernate.DependencyInjection;

public interface INHibernateSetupExtension
{
    public void AddServices(IServiceCollection services);
}
