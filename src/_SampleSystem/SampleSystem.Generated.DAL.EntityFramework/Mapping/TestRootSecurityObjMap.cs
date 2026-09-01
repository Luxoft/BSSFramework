using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDependency;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestRootSecurityObjMap : SampleSystemBaseMap<TestRootSecurityObj>
{
    public override void Configure(EntityTypeBuilder<TestRootSecurityObj> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestRootSecurityObj");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnitFluentMapping).WithMany().HasForeignKey("managementUnitFluentMappingId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Items).WithOne(x => x.FirstMaster).HasForeignKey("firstMasterId").OnDelete(DeleteBehavior.Cascade);
    }
}
