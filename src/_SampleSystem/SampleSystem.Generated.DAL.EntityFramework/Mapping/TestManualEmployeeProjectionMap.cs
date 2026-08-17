using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.ManualProjections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestManualEmployeeProjectionMap : IEntityTypeConfiguration<TestManualEmployeeProjection>
{
    public void Configure(EntityTypeBuilder<TestManualEmployeeProjection> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.CoreBusinessUnitId).IsRequired();
        builder.Property(x => x.Login).IsRequired();
    }
}
