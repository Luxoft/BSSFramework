using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeAutoPropCoreBusinessUnitMap : IEntityTypeConfiguration<TestEmployee_AutoProp_CoreBusinessUnit>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_CoreBusinessUnit> builder)
    {
        builder.ToTable("BusinessUnit", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name_Last_CoreBusinessUnitName);
        builder.Property(x => x.PeriodEndDate_Last_BuEndDate);
        builder.HasMany(x => x.Projects_Last_CoreBusinessUnitProjects).WithOne().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Cascade);
    }
}
