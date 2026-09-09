using Microsoft.EntityFrameworkCore;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.AuditDomain;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemEnversAuditDbContext(DbContextOptions<SampleSystemEnversAuditDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new BusinessUnitAuditMap());
        builder.ApplyConfiguration(new SampleSystemAuditRevisionEntityMap());

        builder.HasDefaultSchema("app");
    }
}
