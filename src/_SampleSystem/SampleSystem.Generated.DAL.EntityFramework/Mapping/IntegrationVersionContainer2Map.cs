using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.IntegrationVersions;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class IntegrationVersionContainer2Map : SampleSystemBaseMap<IntegrationVersionContainer2>
{
    public override void Configure(EntityTypeBuilder<IntegrationVersionContainer2> builder)
    {
        base.Configure(builder);
        builder.ToTable("IntegrationVersionContainer2", "dbo");
        builder.Property(x => x.IntegrationVersion);
        builder.Property(x => x.Name);
    }
}
