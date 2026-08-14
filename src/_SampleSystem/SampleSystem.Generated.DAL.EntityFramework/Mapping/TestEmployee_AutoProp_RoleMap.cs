using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployee_AutoProp_RoleMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Role>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Role> builder)
    {
        builder.ToTable("EmployeeRole", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name_Last_RoleName);
    }
}
