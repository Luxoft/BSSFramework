using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.ExternalPrincipal;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class PrincipalMap : SampleSystemBaseMap<Principal>
{
    public override void Configure(EntityTypeBuilder<Principal> builder)
    {
        base.Configure(builder);
        builder.ToTable("Principal", "dbo");
        builder.Property(x => x.ExternalId);
    }
}
