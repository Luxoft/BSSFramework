using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestEmployeeAutoPropPositionMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Position>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Position> builder)
    {
        builder.ToView(nameof(TestEmployee_AutoProp_Position));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name_Last_PositionName).HasColumnName("Name");
    }
}
