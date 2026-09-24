using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.InlineAudit.DependencyInjection;

public static class InlineAuditDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddInlineAudit(this DbContextOptionsBuilder optionsBuilder, Action<IInlineAuditExtensionSetup> setupAction)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var extension = optionsBuilder.Options.FindExtension<InlineAuditOptionsExtension>()
                        ?? new InlineAuditOptionsExtension(setupAction);

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}
