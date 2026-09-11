using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.ManualProjections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.ManualProjections;

public class TestManualEmployeeProjectionMap : IEntityTypeConfiguration<TestManualEmployeeProjection>
{
    public void Configure(EntityTypeBuilder<TestManualEmployeeProjection> builder)
    {
        builder.ToView(nameof(TestManualEmployeeProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Login).HasColumnName("login");
        builder.Property(x => x.CoreBusinessUnitId).HasColumnName("coreBusinessUnitId");
    }
}
