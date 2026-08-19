using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.IntegrationVersions;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class IntegrationVersionContainer1Map : SampleSystemBaseMap<IntegrationVersionContainer1>
{
    public override void Configure(EntityTypeBuilder<IntegrationVersionContainer1> builder)
    {
        base.Configure(builder);
        builder.ToTable("IntegrationVersionContainer1", "dbo");
        builder.Property(x => x.IntegrationVersion).IsRequired();
        builder.Property(x => x.Name).IsRequired();
    }
}
