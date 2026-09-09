using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestDepartmentMap : IEntityTypeConfiguration<TestDepartment>
{
    public void Configure(EntityTypeBuilder<TestDepartment> builder)
    {
        builder.ToView(nameof(TestDepartment));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("locationId_TestDepartment").HasColumnName("locationId");
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId_TestDepartment");
        builder.HasOne(x => x.Location_Auto).WithMany().HasForeignKey("locationId_TestDepartment");
    }
}
