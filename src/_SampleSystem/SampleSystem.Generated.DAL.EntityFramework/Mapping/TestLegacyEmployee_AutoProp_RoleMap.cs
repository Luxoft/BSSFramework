using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLegacyEmployeeAutoPropRoleMap : IEntityTypeConfiguration<TestLegacyEmployee_AutoProp_Role>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee_AutoProp_Role> builder)
    {
        builder.ToTable("EmployeeRole", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(EmployeeRole)).WithOne().HasForeignKey(typeof(TestLegacyEmployee_AutoProp_Role), nameof(TestLegacyEmployee_AutoProp_Role.Id));
        builder.Property(x => x.Name_Last_RoleName).IsRequired();
    }
}
