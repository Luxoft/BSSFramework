using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestCustomContextSecurityObjProjectionMap : IEntityTypeConfiguration<TestCustomContextSecurityObjProjection>
{
    public void Configure(EntityTypeBuilder<TestCustomContextSecurityObjProjection> builder)
    {
        builder.ToView(nameof(TestCustomContextSecurityObjProjection));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
