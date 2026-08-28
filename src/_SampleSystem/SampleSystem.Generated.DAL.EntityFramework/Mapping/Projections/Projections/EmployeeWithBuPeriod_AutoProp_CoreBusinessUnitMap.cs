using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeWithBuPeriodAutoPropCoreBusinessUnitMap : IEntityTypeConfiguration<EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit>
{
    public void Configure(EntityTypeBuilder<EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit> builder)
    {
        builder.ToView("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit), nameof(EmployeeWithBuPeriod_AutoProp_CoreBusinessUnit.Id)).IsRequired();
        builder.ComplexProperty(x => x.Period_Last_BuPeriod, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("periodendDate");
            period.Property(x => x.StartDate).HasColumnName("periodstartDate");
        });
    }
}
