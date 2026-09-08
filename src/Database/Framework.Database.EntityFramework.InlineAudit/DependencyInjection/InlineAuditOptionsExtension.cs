using Anch.DependencyInjection;

using Framework.Database.InlineAudit.DependencyInjection;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public sealed class InlineAuditOptionsExtension(Action<IInlineAuditSetup> setupAction) : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info => field ??= new ExtensionInfo(this);

    public void ApplyServices(IServiceCollection services) => services.AddScoped<IInterceptor, AuditFlushInterceptor>()
                                                                      .AddServiceProxyFactory()
                                                                      .AddSingleton<IAuditPropertiesSetterMapFactory, AuditPropertiesSetterMapFactory>()
                                                                      .Initialize<InlineAuditSetup>(setupAction);

    public void Validate(IDbContextOptions options)
    {
    }

    private sealed class ExtensionInfo(IDbContextOptionsExtension extension)
        : DbContextOptionsExtensionInfo(extension)
    {
        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "using Audit ";

        public override int GetServiceProviderHashCode() => 0;

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) => debugInfo["Audit"] = "1";

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other) =>
            other is ExtensionInfo;
    }
}
