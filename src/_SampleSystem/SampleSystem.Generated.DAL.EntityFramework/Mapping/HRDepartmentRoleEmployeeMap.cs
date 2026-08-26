using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class HRDepartmentRoleEmployeeMap : SampleSystemBaseMap<HRDepartmentRoleEmployee>
{
    public override void Configure(EntityTypeBuilder<HRDepartmentRoleEmployee> builder)
    {
        base.Configure(builder);
        builder.ToTable("HRDepartmentRoleEmployee", "dbo");
        builder.Property(x => x.HRDepartmentEmployeeRoleType).IsRequired();
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.HRDepartment).WithMany(x => x.HrDepartmentRoleEmployees).HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("employeeId", "hRDepartmentId", nameof(HRDepartmentRoleEmployee.HRDepartmentEmployeeRoleType)).IsUnique().HasDatabaseName("UIX_employee_hRDepartment_hRDepartmentEmployeeRoleTypeHRDepartmentRoleEmployee");
    }
}
