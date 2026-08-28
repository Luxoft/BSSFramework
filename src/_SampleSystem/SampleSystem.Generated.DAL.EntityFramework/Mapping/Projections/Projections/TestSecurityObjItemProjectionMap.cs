using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;
using SampleSystem.Domain.TestDependency;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecurityObjItemProjectionMap : IEntityTypeConfiguration<TestSecurityObjItemProjection>
{
    public void Configure(EntityTypeBuilder<TestSecurityObjItemProjection> builder)
    {
        builder.ToView(nameof(TestSecurityObjItem));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(TestSecurityObjItem)).WithOne().HasForeignKey(typeof(TestSecurityObjItemProjection), nameof(TestSecurityObjItemProjection.Id));
    }
}
