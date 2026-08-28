using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.ManualProjections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestManualEmployeeProjectionMap : IEntityTypeConfiguration<TestManualEmployeeProjection>
{
    public void Configure(EntityTypeBuilder<TestManualEmployeeProjection> builder)
    {
        builder.ToTable("Employee");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestManualEmployeeProjection), nameof(TestManualEmployeeProjection.Id));
        builder.Ignore(x => x.CoreBusinessUnitId);
        builder.Ignore(x => x.Login);
    }
}
