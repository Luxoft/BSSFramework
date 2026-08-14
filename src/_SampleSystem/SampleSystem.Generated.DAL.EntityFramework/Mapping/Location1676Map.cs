using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.EnversBug1676;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class Location1676Map : SampleSystemBaseMap<Location1676>
{
    public override void Configure(EntityTypeBuilder<Location1676> builder)
    {
        base.Configure(builder);
        builder.ToTable("Location1676", "dbo");
        builder.Property(x => x.Name);
        builder.HasMany(x => x.Calendar).WithOne(x => x.Location).HasForeignKey("locationId").OnDelete(DeleteBehavior.Cascade);
    }
}
