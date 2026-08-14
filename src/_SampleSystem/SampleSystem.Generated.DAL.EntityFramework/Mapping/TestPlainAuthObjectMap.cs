using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDeserializedAuth;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestPlainAuthObjectMap : SampleSystemBaseMap<TestPlainAuthObject>
{
    public override void Configure(EntityTypeBuilder<TestPlainAuthObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestPlainAuthObject", "dbo");
        builder.Property(x => x.Name);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Items).WithOne(x => x.Master).HasForeignKey("masterId").OnDelete(DeleteBehavior.Cascade);
    }
}
