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
        builder.ComplexProperty(x => x.EducationDuration, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("educationDurationendDate");
            period.Property(x => x.StartDate).HasColumnName("educationDurationstartDate");
        });
        builder.ComplexProperty(x => x.WorkPeriod, period =>
        {
            period.Property(x => x.EndDate).HasColumnName("workPeriodendDate");
            period.Property(x => x.StartDate).HasColumnName("workPeriodstartDate");
        });
        builder.ComplexProperty(x => x.NameEng, nameEng =>
        {
            nameEng.IsRequired(false);
            nameEng.Property(x => x.FirstName).HasColumnName("nameEngfirstName").HasMaxLength(50);
            nameEng.Property(x => x.LastName).HasColumnName("nameEnglastName").HasMaxLength(50);
        });
        builder.ComplexProperty(x => x.NameNative, nameNative =>
        {
            nameNative.IsRequired(false);
            nameNative.Property(x => x.FirstName).HasColumnName("nameNativefirstName").HasMaxLength(50);
            nameNative.Property(x => x.LastName).HasColumnName("nameNativelastName").HasMaxLength(50);
            nameNative.Property(x => x.MiddleName).HasColumnName("nameNativemiddleName").HasMaxLength(50);
        });
        builder.ComplexProperty(x => x.NameRussian, nameRussian =>
        {
            nameRussian.IsRequired(false);
            nameRussian.Property(x => x.FirstName).HasColumnName("nameRussianfirstName").HasMaxLength(50);
            nameRussian.Property(x => x.LastName).HasColumnName("nameRussianlastName").HasMaxLength(50);
            nameRussian.Property(x => x.MiddleName).HasColumnName("nameRussianmiddleName").HasMaxLength(50);
        });
        builder.HasOne(x => x.Role).WithMany().HasForeignKey("roleId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RoleDegree).WithMany().HasForeignKey("roleDegreeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RegistrationType).WithMany().HasForeignKey("registrationTypeId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.VacationApprover).WithMany().HasForeignKey("vacationApproverId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CoreBusinessUnit).WithMany().HasForeignKey("coreBusinessUnitId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.HRDepartment).WithMany().HasForeignKey("hRDepartmentId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnit).WithMany().HasForeignKey("managementUnitId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Position).WithMany().HasForeignKey("positionId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Ppm).WithMany().HasForeignKey("ppmId").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}
