using Anch.GenericQueryable.DependencyInjection;
using Anch.GenericQueryable.EntityFramework;

using Framework.BLL.Visitors;
using Framework.Core;
using Framework.Database;
using Framework.Database.DependencyInjection;
using Framework.Database.EntityFramework;
using Framework.Database.EntityFramework.DependencyInjection;
using Framework.Database.EntityFramework.EnversAudit.DependencyInjection;
using Framework.Database.EntityFramework.Extensions;
using Framework.Database.EntityFramework.InlineAudit.DependencyInjection;
using Framework.Database.Mapping;
using Framework.Infrastructure.DependencyInjection;
using Framework.Projection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemEntityFrameworkExtension : IBssFrameworkExtension
{
    public void AddServices(IServiceCollection services) =>
        services
            .AddDbContext<SampleSystemDbContext>((sp, options) => options
                                                                  .UseSqlServer(sp.GetRequiredService<IDefaultConnectionStringSource>().ConnectionString)
                                                                  .UseGenericQueryable(s => s.SetSetupType<SampleSystemGenericQueryableSetup>())
                                                                  .UseLazyLoadingProxies()
                                                                  .IgnoreComputedProperties()
                                                                  .AddEnversAudit(s => s.SetSetupType<SampleSystemEnversAuditSetup>())
                                                                  .AddInlineAudit(ias => ias.AddSampleSystemInlineAudit()))

            .AddDbContext<SampleSystemEnversAuditDbContext>((sp, options) => options
                                                                             .UseSqlServer(
                                                                                 sp.GetRequiredService<IDefaultConnectionStringSource>().ConnectionString)
                                                                             .UseGenericQueryable()
                                                                             .UseLazyLoadingProxies()
                                                                             .IgnoreComputedProperties())

            .AddEntityFramework<SampleSystemDbContext>(s => s.AddEnversAudit()
                                                             .AddLegacyDatabaseSettings()
                                                             .AddSecondaryContext<SampleSystemEnversAuditDbContext>());

    private class SampleSystemGenericQueryableSetup : IEfGenericQueryableExtensionInnerSetup
    {
        public void Initialize(IGenericQueryableSetup setup) =>
            setup.SetVisitor<RootExpressionVisitor>()
                 .AddServices(sc => sc.AddDatabaseVisitors(dvs => dvs.AddVisitor<ExpandPathVisitor>()
                                                                     .AddVisitor<TestEmployeeExpressionVisitor>()));
    }

    private class SampleSystemEnversAuditSetup : IEnversAuditExtensionInnerSetup
    {
        public void Initialize(IEnversAuditSetup setup) =>
            setup.SetFilter(et => !et.ClrType.IsProjection()
                                  && !et.ClrType
                                        .HasAttribute<NotAuditedClassAttribute>());
    }
}
