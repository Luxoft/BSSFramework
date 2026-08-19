using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDependency;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecurityObjItemMap : SampleSystemBaseMap<TestSecurityObjItem>
{
    public override void Configure(EntityTypeBuilder<TestSecurityObjItem> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestSecurityObjItem", "dbo");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.FirstMaster).WithMany(x => x.Items).HasForeignKey("firstMasterId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Items).WithOne(x => x.InnerMaster).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Items2).WithOne(x => x.InnerMaster).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.Items3).WithOne(x => x.InnerMaster).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Cascade);
    }
}
