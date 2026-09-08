using Anch.Core.Auth;

using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;
using Framework.Database.EntityFramework.EnversAudit;
using Framework.Database.EntityFramework.InlineAudit;
using Framework.Database.EntityFramework.Sessions;

using Microsoft.EntityFrameworkCore;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemDbContext(
    DbContextOptions<SampleSystemDbContext> options,
    TimeProvider timeProvider,
    ICurrentUser currentUser,
    EfCurrentRevisionState currentRevisionState,
    IServiceProvider serviceProvider)
    : DbContext(options), IEnversAuditDbContext, IInlineAuditDbContext
{
    public TimeProvider TimeProvider { get; } = timeProvider;

    public ICurrentUser CurrentUser { get; } = currentUser;

    public EfCurrentRevisionState CurrentRevisionState { get; } = currentRevisionState;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Ignore<SampleSystem.Domain.Inline.Fio>();
        builder.Ignore<SampleSystem.Domain.Inline.FioShort>();

        builder.ApplyConfigurationsFromAssembly(typeof(AuthBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(ConfigurationBaseMap<>).Assembly);
        builder.ApplyConfigurationsFromAssembly(typeof(SampleSystemBaseMap<>).Assembly, t => !t.Namespace!.Contains("Projection"));

        builder.HasDefaultSchema("app");
    }

    public IServiceProvider ServiceProvider { get; } = serviceProvider;
}
