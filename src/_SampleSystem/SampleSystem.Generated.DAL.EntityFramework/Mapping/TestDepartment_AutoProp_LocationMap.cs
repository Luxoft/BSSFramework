using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestDepartmentAutoPropLocationMap : IEntityTypeConfiguration<TestDepartment_AutoProp_Location>
{
    public void Configure(EntityTypeBuilder<TestDepartment_AutoProp_Location> builder)
    {
        builder.ToTable("Location");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Location)).WithOne().HasForeignKey(typeof(TestDepartment_AutoProp_Location), nameof(TestDepartment_AutoProp_Location.Id));
        builder.Property(x => x.BinaryData_Last_LocationBinaryData).HasMaxLength(int.MaxValue);
    }
}
