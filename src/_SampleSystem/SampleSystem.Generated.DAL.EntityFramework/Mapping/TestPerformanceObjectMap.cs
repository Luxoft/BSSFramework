using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestDeserializedAuth;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestPerformanceObjectMap : SampleSystemBaseMap<TestPerformanceObject>
{
    public override void Configure(EntityTypeBuilder<TestPerformanceObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestPerformanceObject");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnit).WithMany().HasForeignKey("managementUnitId").OnDelete(DeleteBehavior.Restrict);
    }
}
