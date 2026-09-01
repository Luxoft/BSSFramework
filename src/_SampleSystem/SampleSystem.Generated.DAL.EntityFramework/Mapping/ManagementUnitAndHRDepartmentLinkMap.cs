using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.MU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitAndHRDepartmentLinkMap : SampleSystemBaseMap<ManagementUnitAndHRDepartmentLink>
{
    public override void Configure(EntityTypeBuilder<ManagementUnitAndHRDepartmentLink> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnitAndHRDepartmentLink");
        builder.HasOne(x => x.HRDepartment).WithMany(x => x.ManagementUnits).HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnit).WithMany(x => x.HRDepartments).HasForeignKey("managementUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("hRDepartmentId", "managementUnitId").IsUnique().HasDatabaseName("UIX_hRDepartment_managementUnitManagementUnitAndHRDepartmentLink");
    }
}
