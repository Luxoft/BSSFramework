using Framework.Core;
using Framework.Database;
using Framework.Database.EntityFramework.Audit.DependencyInjection;
using Framework.Database.EntityFramework.DependencyInjection;
using Framework.Database.EntityFramework.Extensions;
using Framework.Database.Mapping;
using Framework.Infrastructure.DependencyInjection;
using Framework.Projection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemEntityFrameworkExtension : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) =>
        services.AddEntityFramework(s => s.SetDbContext<SampleSystemDbContext>())
                .AddDbContext<SampleSystemDbContext>((sp, options) => options
                                                                      .UseSqlServer(sp.GetRequiredService<IDefaultConnectionStringSource>().ConnectionString)
                                                                      .UseLazyLoadingProxies()
                                                                      .IgnoreComputedProperties()
                                                                      .AddAudit(auditSetup =>
                                                                                    auditSetup.SetFilter(et => !et.ClrType.IsProjection()
                                                                                                             && !et.ClrType
                                                                                                                 .HasAttribute<NotAuditedClassAttribute>())));
}
