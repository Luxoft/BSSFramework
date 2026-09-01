using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDeserializedAuth;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestItemAuthObjectMap : SampleSystemBaseMap<TestItemAuthObject>
{
    public override void Configure(EntityTypeBuilder<TestItemAuthObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestItemAuthObject");
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnit).WithMany().HasForeignKey("managementUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Master).WithMany(x => x.Items).HasForeignKey("masterId").OnDelete(DeleteBehavior.Restrict);
    }
}
