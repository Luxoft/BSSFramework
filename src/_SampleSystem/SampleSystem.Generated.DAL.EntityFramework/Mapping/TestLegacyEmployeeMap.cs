using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLegacyEmployeeMap : IEntityTypeConfiguration<TestLegacyEmployee>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Login).IsRequired();
        builder.Property(x => x.Login_Security).IsRequired();
        builder.HasOne(x => x.BusinessUnit_Security).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Department_Security).WithMany().HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId").OnDelete(DeleteBehavior.Restrict);
    }
}
