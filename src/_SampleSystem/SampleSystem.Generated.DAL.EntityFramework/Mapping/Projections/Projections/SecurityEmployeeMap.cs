using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SecurityEmployeeMap : IEntityTypeConfiguration<SecurityEmployee>
{
    public void Configure(EntityTypeBuilder<SecurityEmployee> builder)
    {
        builder.ToView(nameof(Employee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(SecurityEmployee), nameof(SecurityEmployee.Id));

        builder.Property(x => x.Login_Security).HasColumnName("Login");
        builder.Property<System.Guid?>("coreBusinessUnitId_SecurityEmployee").HasColumnName("coreBusinessUnitId");
        builder.HasOne(x => x.BusinessUnit_Security).WithMany().HasForeignKey("coreBusinessUnitId_SecurityEmployee");
        builder.Property<System.Guid?>("hRDepartmentId_SecurityEmployee").HasColumnName("hRDepartmentId");
        builder.HasOne(x => x.Department_Security).WithMany().HasForeignKey("hRDepartmentId_SecurityEmployee");
    }
}
