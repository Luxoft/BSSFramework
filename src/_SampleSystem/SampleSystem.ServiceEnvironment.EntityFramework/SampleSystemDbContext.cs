using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Database.EntityFramework.EnversAudit;
using Framework.Database.EntityFramework.InlineAudit;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemDbContext(DbContextOptions<SampleSystemDbContext> options, IServiceProvider serviceProvider)
    : DbContext(options), IEnversAuditDbContext, IInlineAuditDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<SampleSystem.Domain.Inline.Fio>();
        builder.Ignore<SampleSystem.Domain.Inline.FioShort>();

        builder.ApplyConfigurationsFromAssembly(typeof(AuthBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ConfigurationBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(
            typeof(SampleSystemBaseMap<>).Assembly,
            t => !t.Namespace!.Contains("Projection") && !t.Namespace!.Contains("Envers"));

        //builder.ApplyConfiguration(new TestBusinessUnitMap());

        builder.HasDefaultSchema("app");
    }

    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
