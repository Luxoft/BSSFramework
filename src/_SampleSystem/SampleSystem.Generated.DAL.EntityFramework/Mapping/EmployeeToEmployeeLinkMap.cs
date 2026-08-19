using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee.EmpoloyeeLink;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class EmployeeToEmployeeLinkMap : SampleSystemBaseMap<EmployeeToEmployeeLink>
{
    public override void Configure(EntityTypeBuilder<EmployeeToEmployeeLink> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.EmployeeLinkType).IsRequired();
        builder.HasOne(x => x.Owner).WithMany(x => x.EmployeeToEmployeeLinks).HasForeignKey("ownerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.LinkedEmployee).WithMany().HasForeignKey("linkedEmployeeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
