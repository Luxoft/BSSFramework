using Anch.DependencyInjection;

using Framework.Database.InlineAudit.DependencyInjection;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public sealed class InlineAuditOptionsExtension(Action<IInlineAuditExtensionSetup> setupAction) : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info => field ??= new ExtensionInfo(this);

    private InlineAuditExtensionSetup ExtensionSetup
    {
        get
        {
            if (field == null)
            {
                field = new InlineAuditExtensionSetup();
                setupAction.Invoke(field);
            }

            return field;
        }
    }

    public void ApplyServices(IServiceCollection services) => services.AddScoped<IInterceptor, AuditFlushInterceptor>()
                                                                      .AddServiceProxyFactory()
                                                                      .AddSingleton<IAuditPropertiesSetterMapFactory, AuditPropertiesSetterMapFactory>()
                                                                      .Initialize<InlineAuditSetup>(s => this.ExtensionSetup.CreateInstance?.Invoke()
                                                                                                        .Initialize(s));

    public void Validate(IDbContextOptions options)
    {
    }

    private sealed class ExtensionInfo(InlineAuditOptionsExtension extension)
        : DbContextOptionsExtensionInfo(extension)
    {
        private InlineAuditExtensionSetup ExtensionSetup => extension.ExtensionSetup;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "using Inline Audit ";

        public override int GetServiceProviderHashCode() => (this.ExtensionSetup.SetupType ?? typeof(ExtensionInfo)).GetHashCode();

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo untypedOther)
        {
            return untypedOther is ExtensionInfo other && this.ExtensionSetup.SetupType == other.ExtensionSetup.SetupType;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) =>
            debugInfo[nameof(InlineAudit)] = this.GetServiceProviderHashCode().ToString();
    }
}
