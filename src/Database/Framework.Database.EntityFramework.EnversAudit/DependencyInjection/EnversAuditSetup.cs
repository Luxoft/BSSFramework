using Anch.DependencyInjection;

using Framework.Database.EntityFramework.Extensions;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public class EnversAuditSetup : IEnversAuditSetup, IServiceInitializer
{
    private Action<IServiceCollection> initFilterAction = sc => sc.AddSingleton<IAuditableEntityFilter, AuditableEntityFilter>();

    public IEnversAuditSetup SetFilter(Func<IReadOnlyEntityType, bool> isAuditable)
    {
        this.initFilterAction = sc => sc.AddSingleton<IAuditableEntityFilter>(new CustomAuditableEntityFilter(isAuditable));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped<IInterceptor, AuditFlushInterceptor>();

        services.AddSingleton<IAuditEntityFactory, AuditEntityFactory>();
        services.AddSingleton<IAuditInfoResolver, AuditInfoResolver>();
        services.AddSingleton<IAuditTypeNameResolver, AuditTypeNameResolver>();
        services.AddSingleton(MainAuditSchemaInfo.Default);

        services.ReplaceSingleton<IModelCustomizer, RootModelCustomizer>()
                .AddKeyedSingleton<IModelCustomizer, AuditModelCustomizer>(RootModelCustomizer.ElementKey);

        this.initFilterAction(services);
    }
}
