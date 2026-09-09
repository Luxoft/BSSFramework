using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.LegacyProjections;

public class TestLegacyEmployeeAutoPropRoleMap : IEntityTypeConfiguration<TestLegacyEmployee_AutoProp_Role>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee_AutoProp_Role> builder)
    {
        builder.ToView(nameof(TestLegacyEmployee_AutoProp_Role));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name_Last_RoleName).HasColumnName("Name");
    }
}
