using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitEmployeeRoleMap : SampleSystemBaseMap<BusinessUnitEmployeeRole>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitEmployeeRole> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Role).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany(x => x.BusinessUnitEmployeeRoles).HasForeignKey("businessUnitId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
