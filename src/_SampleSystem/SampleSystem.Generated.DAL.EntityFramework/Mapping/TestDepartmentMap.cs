using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestDepartmentMap : IEntityTypeConfiguration<TestDepartment>
{
    public void Configure(EntityTypeBuilder<TestDepartment> builder)
    {
        builder.ToTable("HRDepartment", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(HRDepartment)).WithOne().HasForeignKey(typeof(TestDepartment), nameof(TestDepartment.Id));
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Location_Auto).WithMany().HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
    }
}
