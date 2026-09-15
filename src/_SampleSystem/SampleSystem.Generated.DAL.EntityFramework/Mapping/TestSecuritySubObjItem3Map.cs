using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDependency;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecuritySubObjItem3Map : SampleSystemBaseMap<TestSecuritySubObjItem3>
{
    public override void Configure(EntityTypeBuilder<TestSecuritySubObjItem3> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestSecuritySubObjItem3");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.InnerMaster).WithMany(x => x.Items3).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Restrict);
    }
}
