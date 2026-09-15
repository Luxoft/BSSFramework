using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.EnversAudit.DependencyInjection;

public static class EnversAuditDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddEnversAudit(this DbContextOptionsBuilder optionsBuilder, Action<IEnversAuditSetup>? setupAction = null)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var extension = optionsBuilder.Options.FindExtension<EnversAuditOptionsExtension>()
                        ?? new EnversAuditOptionsExtension(setupAction);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}
