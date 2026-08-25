using Microsoft.EntityFrameworkCore;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemDbContext(DbContextOptions<SampleSystemDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(SampleSystemBaseMap<>).Assembly);

        builder.HasDefaultSchema("app");
    }
}
