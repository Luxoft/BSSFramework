using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee.EmpoloyeeLink;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class EmployeeAndEmployeeSpecializationLinkMap : SampleSystemBaseMap<EmployeeAndEmployeeSpecializationLink>
{
    public override void Configure(EntityTypeBuilder<EmployeeAndEmployeeSpecializationLink> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.Employee).WithMany(x => x.Specializations).HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Specialization).WithMany().HasForeignKey("specializationId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("employeeId", "specializationId").IsUnique().HasDatabaseName("UIX_employee_specializationEmployeeAndEmployeeSpecializationLink");
    }
}
