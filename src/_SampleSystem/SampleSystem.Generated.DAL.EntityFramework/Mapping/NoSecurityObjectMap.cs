using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class NoSecurityObjectMap : SampleSystemBaseMap<NoSecurityObject>
{
    public override void Configure(EntityTypeBuilder<NoSecurityObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("NoSecurityObject", "dbo");
    }
}
