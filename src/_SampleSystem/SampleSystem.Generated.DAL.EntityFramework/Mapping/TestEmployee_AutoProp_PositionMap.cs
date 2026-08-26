using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeAutoPropPositionMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Position>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Position> builder)
    {
        builder.ToTable("EmployeePosition");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(EmployeePosition)).WithOne().HasForeignKey(typeof(TestEmployee_AutoProp_Position), nameof(TestEmployee_AutoProp_Position.Id));
        builder.Property(x => x.Name_Last_PositionName).IsRequired();
    }
}
