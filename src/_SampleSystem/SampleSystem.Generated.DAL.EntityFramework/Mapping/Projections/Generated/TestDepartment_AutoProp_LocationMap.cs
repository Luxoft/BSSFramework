using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestDepartmentAutoPropLocationMap : IEntityTypeConfiguration<TestDepartment_AutoProp_Location>
{
    public void Configure(EntityTypeBuilder<TestDepartment_AutoProp_Location> builder)
    {
        builder.ToView(nameof(TestDepartment_AutoProp_Location));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.BinaryData_Last_LocationBinaryData).HasColumnName("BinaryData");
    }
}
