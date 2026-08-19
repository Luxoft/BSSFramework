using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeMap : SampleSystemBaseMap<Employee>
{
    public override void Configure(EntityTypeBuilder<Employee> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
        builder.Property(x => x.Login).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Interphone).HasMaxLength(25).IsRequired();
        builder.Property(x => x.ExternalId).IsRequired();
        builder.HasIndex(x => x.Login).IsUnique();
        builder.HasOne(x => x.Role).WithMany().HasForeignKey("roleId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RoleDegree).WithMany().HasForeignKey("roleDegreeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RegistrationType).WithMany().HasForeignKey("registrationTypeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.VacationApprover).WithMany().HasForeignKey("vacationApproverId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
