using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeAutoPropPositionMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Position>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Position> builder)
    {
        builder.ToTable("EmployeePosition", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name_Last_PositionName);
    }
}
