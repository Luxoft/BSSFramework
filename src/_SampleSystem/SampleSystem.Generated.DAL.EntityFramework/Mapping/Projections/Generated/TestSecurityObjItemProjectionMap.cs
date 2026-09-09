using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;
using SampleSystem.Domain.TestDependency;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestSecurityObjItemProjectionMap : IEntityTypeConfiguration<TestSecurityObjItemProjection>
{
    public void Configure(EntityTypeBuilder<TestSecurityObjItemProjection> builder)
    {
        builder.ToView(nameof(TestSecurityObjItemProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
    }
}
