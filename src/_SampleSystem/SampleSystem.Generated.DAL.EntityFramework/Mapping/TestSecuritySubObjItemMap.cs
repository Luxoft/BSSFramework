using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDependency;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecuritySubObjItemMap : SampleSystemBaseMap<TestSecuritySubObjItem>
{
    public override void Configure(EntityTypeBuilder<TestSecuritySubObjItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestSecuritySubObjItem");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.InnerMaster).WithMany(x => x.Items).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Restrict);
    }
}
