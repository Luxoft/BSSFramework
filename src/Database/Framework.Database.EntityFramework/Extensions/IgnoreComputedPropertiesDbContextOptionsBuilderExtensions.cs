using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Framework.Database.EntityFramework.Extensions;

public static class IgnoreComputedPropertiesDbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder IgnoreComputedProperties(this DbContextOptionsBuilder optionsBuilder)
    {
        ArgumentNullException.ThrowIfNull(optionsBuilder);

        var extension = optionsBuilder.Options.FindExtension<IgnoreComputedPropertiesOptionsExtension>()
                        ?? new IgnoreComputedPropertiesOptionsExtension();

        ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

        return optionsBuilder;
    }
}
