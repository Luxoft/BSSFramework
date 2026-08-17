using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDependency;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecuritySubObjItem2Map : SampleSystemBaseMap<TestSecuritySubObjItem2>
{
    public override void Configure(EntityTypeBuilder<TestSecuritySubObjItem2> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestSecuritySubObjItem2", "dbo");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.InnerMaster).WithMany(x => x.Items2).HasForeignKey("innerMasterId").OnDelete(DeleteBehavior.Restrict);
    }
}
