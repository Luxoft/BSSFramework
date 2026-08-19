using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestDepartmentAutoPropLocationMap : IEntityTypeConfiguration<TestDepartment_AutoProp_Location>
{
    public void Configure(EntityTypeBuilder<TestDepartment_AutoProp_Location> builder)
    {
        builder.ToTable("Location", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.BinaryData_Last_LocationBinaryData).HasMaxLength(int.MaxValue);
    }
}
