using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Projection;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.ServiceEnvironment.DependencyInjection;

public class SampleSystemDbContext(DbContextOptions<SampleSystemDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<SampleSystem.Domain.Inline.Fio>();
        builder.Ignore<SampleSystem.Domain.Inline.FioShort>();

        builder.ApplyConfigurationsFromAssembly(typeof(AuthBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ConfigurationBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(SampleSystemBaseMap<>).Assembly, t => !t.Namespace!.Contains("Projection"));

        builder.HasDefaultSchema("app");
    }
}
