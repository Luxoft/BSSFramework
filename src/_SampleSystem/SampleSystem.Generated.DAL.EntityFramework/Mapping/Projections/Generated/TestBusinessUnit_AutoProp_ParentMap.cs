using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestBusinessUnitAutoPropParentMap : IEntityTypeConfiguration<TestBusinessUnit_AutoProp_Parent>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnit_AutoProp_Parent> builder)
    {
        builder.ToView(nameof(BusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(TestBusinessUnit_AutoProp_Parent), nameof(TestBusinessUnit_AutoProp_Parent.Id)).IsRequired();
        builder.Property(x => x.PeriodStartDate_Last_ParentPeriodStartDate).HasColumnName("periodstartDate").IsRequired();
        builder.ComplexProperty(x => x.Period_Last_ParentPeriod, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("periodendDate");
            period.Property(x => x.StartDate).HasColumnName("periodstartDate");
        });
    }
}
