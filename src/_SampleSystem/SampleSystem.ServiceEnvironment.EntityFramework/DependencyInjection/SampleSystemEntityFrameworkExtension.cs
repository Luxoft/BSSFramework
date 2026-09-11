using Anch.GenericQueryable.EntityFramework;

using Framework.Core;
using Framework.Database;
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
                                                                  .UseGenericQueryable(s => s.SetVisitor(sp.GetRequiredService<IExpressionVisitorContainer>().Visitor)
                                                                                             .AddFetchRuleExpander<Framework.Authorization.BLL.AuthorizationMainDTOFetchRuleExpander>()
                                                                                             .AddFetchRuleExpander<Framework.Configuration.BLL.ConfigurationMainDTOFetchRuleExpander>()
                                                                                             .AddFetchRuleExpander<SampleSystem.BLL.SampleSystemMainDTOFetchRuleExpander>())
                                                                  .UseLazyLoadingProxies()
                                                                  .IgnoreComputedProperties()
                                                                  .AddEnversAudit(auditSetup =>
                                                                                      auditSetup.SetFilter(et => !et.ClrType.IsProjection()
                                                                                          && !et.ClrType
                                                                                                .HasAttribute<NotAuditedClassAttribute>()))
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
}
