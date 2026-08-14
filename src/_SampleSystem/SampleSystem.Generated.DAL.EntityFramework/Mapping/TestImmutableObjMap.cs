using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestImmutableObjMap : SampleSystemBaseMap<TestImmutableObj>
{
    public override void Configure(EntityTypeBuilder<TestImmutableObj> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestImmutableObj", "dbo");
        builder.Property(x => x.TestImmutablePrimitiveProperty);
        builder.HasOne(x => x.TestImmutableRefProperty).WithMany().HasForeignKey("testImmutableRefPropertyId").OnDelete(DeleteBehavior.Restrict);
    }
}
