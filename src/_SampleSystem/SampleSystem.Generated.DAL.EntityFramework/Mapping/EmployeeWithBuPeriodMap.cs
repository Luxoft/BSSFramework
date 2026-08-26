using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeWithBuPeriodMap : IEntityTypeConfiguration<EmployeeWithBuPeriod>
{
    public void Configure(EntityTypeBuilder<EmployeeWithBuPeriod> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(EmployeeWithBuPeriod), nameof(EmployeeWithBuPeriod.Id));
        builder.HasOne(x => x.CoreBusinessUnit_Auto).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
    }
}
