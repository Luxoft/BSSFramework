using Framework.Database;
using Framework.Database.EntityFramework.Audit.DependencyInjection;
using Framework.Infrastructure.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemEntityFrameworkExtension : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) =>
        services.AddDbContext<SampleSystemDbContext>((sp, options) => options
                                                                      .UseSqlServer(sp.GetRequiredService<IDefaultConnectionStringSource>().ConnectionString)
                                                                      .UseLazyLoadingProxies()
                                                                      //.UseChangeTrackingProxies()
                                                                      .AddAudit());
}
