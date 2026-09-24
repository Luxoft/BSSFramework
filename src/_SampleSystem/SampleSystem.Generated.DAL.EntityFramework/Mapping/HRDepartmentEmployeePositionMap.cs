using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class HRDepartmentEmployeePositionMap : SampleSystemBaseMap<HRDepartmentEmployeePosition>
{
    public override void Configure(EntityTypeBuilder<HRDepartmentEmployeePosition> builder)
    {
        base.Configure(builder);
        builder.ToTable("HRDepartmentEmployeePosition");
        builder.HasOne(x => x.EmployeePosition).WithMany().HasForeignKey("employeePositionId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.HrDepartment).WithMany(x => x.EmployeePositions).HasForeignKey("hrDepartmentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("employeePositionId", "hrDepartmentId").IsUnique().HasDatabaseName("UIX_employeePosition_hrDepartmentHRDepartmentEmployeePosition");
    }
}
