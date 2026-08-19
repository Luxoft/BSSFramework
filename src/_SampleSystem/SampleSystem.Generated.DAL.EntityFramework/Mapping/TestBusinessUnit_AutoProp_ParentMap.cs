using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestBusinessUnitAutoPropParentMap : IEntityTypeConfiguration<TestBusinessUnit_AutoProp_Parent>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnit_AutoProp_Parent> builder)
    {
        builder.ToTable("BusinessUnit", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.PeriodStartDate_Last_ParentPeriodStartDate).IsRequired();
        builder.ComplexProperty(x => x.Period_Last_ParentPeriod, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("periodendDate");
            period.Property(x => x.StartDate).HasColumnName("periodstartDate");
        });
    }
}
