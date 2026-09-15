using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitHrDepartmentMap : SampleSystemBaseMap<BusinessUnitHrDepartment>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitHrDepartment> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.HRDepartment).WithMany().HasForeignKey("hRDepartmentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("businessUnitId", "hRDepartmentId").IsUnique().HasDatabaseName("UIX_businessUnit_hRDepartmentBusinessUnitHrDepartment");
    }
}
