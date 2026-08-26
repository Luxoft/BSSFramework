using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SecurityEmployeeMap : IEntityTypeConfiguration<SecurityEmployee>
{
    public void Configure(EntityTypeBuilder<SecurityEmployee> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(SecurityEmployee), nameof(SecurityEmployee.Id));
        builder.Property(x => x.Login_Security).IsRequired();
        builder.HasOne(x => x.BusinessUnit_Security).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Department_Security).WithMany().HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Restrict);
    }
}
