using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLegacyEmployeeAutoPropRoleMap : IEntityTypeConfiguration<TestLegacyEmployee_AutoProp_Role>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee_AutoProp_Role> builder)
    {
        builder.ToTable("EmployeeRole", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name_Last_RoleName).IsRequired();
    }
}
