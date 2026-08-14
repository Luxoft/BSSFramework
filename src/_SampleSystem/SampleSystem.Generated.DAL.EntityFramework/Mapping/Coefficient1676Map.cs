using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.EnversBug1676;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class Coefficient1676Map : SampleSystemBaseMap<Coefficient1676>
{
    public override void Configure(EntityTypeBuilder<Coefficient1676> builder)
    {
        base.Configure(builder);
        builder.ToTable("Coefficient1676", "dbo");
        builder.Property(x => x.NormCoefficient).HasPrecision(19, 4);
        builder.HasOne(x => x.Location).WithOne(x => x.Coefficient).HasForeignKey<Coefficient1676>("locationId").OnDelete(DeleteBehavior.Restrict);
    }
}
