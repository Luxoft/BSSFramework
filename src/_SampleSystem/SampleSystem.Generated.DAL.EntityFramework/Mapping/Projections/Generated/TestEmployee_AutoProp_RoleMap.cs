using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestEmployee_AutoProp_RoleMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Role>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Role> builder)
    {
        builder.ToView(nameof(TestEmployee_AutoProp_Role));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name_Last_RoleName).HasColumnName("Name");
    }
}
