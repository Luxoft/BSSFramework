using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class MiniBusinessUnitEmployeeRoleMap : IEntityTypeConfiguration<MiniBusinessUnitEmployeeRole>
{
    public void Configure(EntityTypeBuilder<MiniBusinessUnitEmployeeRole> builder)
    {
        builder.ToTable("BusinessUnitEmployeeRole", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").OnDelete(DeleteBehavior.Restrict);
    }
}
