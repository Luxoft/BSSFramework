using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCustomContextSecurityObjProjectionMap : IEntityTypeConfiguration<TestCustomContextSecurityObjProjection>
{
    public void Configure(EntityTypeBuilder<TestCustomContextSecurityObjProjection> builder)
    {
        builder.ToView(nameof(TestCustomContextSecurityObj));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(TestCustomContextSecurityObj)).WithOne().HasForeignKey(typeof(TestCustomContextSecurityObjProjection), nameof(TestCustomContextSecurityObjProjection.Id));
    }
}
