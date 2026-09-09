using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class EmployeeWithBuPeriodMap : IEntityTypeConfiguration<EmployeeWithBuPeriod>
{
    public void Configure(EntityTypeBuilder<EmployeeWithBuPeriod> builder)
    {
        builder.ToView(nameof(EmployeeWithBuPeriod));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("coreBusinessUnitId_EmployeeWithBuPeriod").HasColumnName("coreBusinessUnitId");
        builder.HasOne(x => x.CoreBusinessUnit_Auto).WithMany().HasForeignKey("coreBusinessUnitId_EmployeeWithBuPeriod");
    }
}
