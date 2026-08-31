using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.ManualProjections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.ManualProjections;

public class TestManualEmployeeProjectionMap : IEntityTypeConfiguration<TestManualEmployeeProjection>
{
    public void Configure(EntityTypeBuilder<TestManualEmployeeProjection> builder)
    {
        builder.ToView(nameof(Employee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestManualEmployeeProjection), nameof(TestManualEmployeeProjection.Id));
    }
}
