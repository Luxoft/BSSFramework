using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeMap : IEntityTypeConfiguration<TestEmployee>
{
    public void Configure(EntityTypeBuilder<TestEmployee> builder)
    {
        builder.ToTable("Employee");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestEmployee), nameof(TestEmployee.Id));
        builder.Property(x => x.Login).IsRequired();
        builder.Property(x => x.NameEngFirstName).HasColumnName("nameEngfirstName").HasMaxLength(50).IsRequired();
        builder.HasOne(x => x.CoreBusinessUnit_Auto).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Position_Auto).WithMany().HasForeignKey("positionId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Ppm_Auto).WithMany().HasForeignKey("ppmId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId").OnDelete(DeleteBehavior.Restrict);
    }
}
