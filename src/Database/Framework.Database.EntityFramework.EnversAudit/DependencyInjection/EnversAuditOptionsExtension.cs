using Anch.DependencyInjection;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public sealed class EnversAuditOptionsExtension(Action<IEnversAuditExtensionSetup>? setupAction) : IDbContextOptionsExtension
{
    public DbContextOptionsExtensionInfo Info => field ??= new ExtensionInfo(this);

    private EnversAuditExtensionSetup ExtensionSetup
    {
        get
        {
            if (field == null)
            {
                field = new EnversAuditExtensionSetup();
                setupAction?.Invoke(field);
            }

            return field;
        }
    }

    public void ApplyServices(IServiceCollection services) =>
        services.Initialize<EnversAuditSetup>(s => this.ExtensionSetup.CreateInstance?.Invoke().Initialize(s));

    public void Validate(IDbContextOptions options)
    {
    }

    private sealed class ExtensionInfo(EnversAuditOptionsExtension extension)
        : DbContextOptionsExtensionInfo(extension)
    {
        private EnversAuditExtensionSetup ExtensionSetup => extension.ExtensionSetup;

        public override bool IsDatabaseProvider => false;

        public override string LogFragment => "using Audit ";

        public override int GetServiceProviderHashCode() => (this.ExtensionSetup.SetupType ?? typeof(ExtensionInfo)).GetHashCode();

        public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo untypedOther)
        {
            return untypedOther is ExtensionInfo other && this.ExtensionSetup.SetupType == other.ExtensionSetup.SetupType;
        }

        public override void PopulateDebugInfo(IDictionary<string, string> debugInfo) =>
            debugInfo[nameof(EnversAudit)] = this.GetServiceProviderHashCode().ToString();
    }
}
