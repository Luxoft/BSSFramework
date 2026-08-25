using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Audit.DependencyInjection;

public static class AuditDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddAudit(this DbContextOptionsBuilder optionsBuilder, string auditSchema = "appAudit")
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var extension = optionsBuilder.Options.FindExtension<AuditOptionsExtension>()
                        ?? new AuditOptionsExtension(auditSchema);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}
