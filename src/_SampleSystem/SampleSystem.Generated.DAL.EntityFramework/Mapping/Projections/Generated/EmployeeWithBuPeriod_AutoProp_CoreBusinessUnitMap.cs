using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class EmployeeWithBuPeriodAutoPropCoreBusinessUnitMap : IEntityTypeConfiguration<EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit>
{
    public void Configure(EntityTypeBuilder<EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit> builder)
    {
        builder.ToTable(nameof(EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit), t => t.ExcludeFromMigrations());
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ComplexProperty(x => x.Period_Last_BuPeriod, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("periodendDate");
            period.Property(x => x.StartDate).HasColumnName("periodstartDate");
        });
    }
}
