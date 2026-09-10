using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestEmployeeAutoPropCoreBusinessUnitMap : IEntityTypeConfiguration<TestEmployee_AutoProp_CoreBusinessUnit>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_CoreBusinessUnit> builder)
    {
        builder.ToView(nameof(TestEmployee_AutoProp_CoreBusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name_Last_CoreBusinessUnitName).HasColumnName("Name");
        builder.Property(x => x.PeriodEndDate_Last_BuEndDate).HasColumnName("periodendDate");
        builder.HasMany(x => x.Projects_Last_CoreBusinessUnitProjects).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId_VisualProject");
    }
}
