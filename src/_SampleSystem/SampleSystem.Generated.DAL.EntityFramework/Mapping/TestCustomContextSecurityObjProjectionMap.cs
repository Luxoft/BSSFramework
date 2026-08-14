using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCustomContextSecurityObjProjectionMap : IEntityTypeConfiguration<TestCustomContextSecurityObjProjection>
{
    public void Configure(EntityTypeBuilder<TestCustomContextSecurityObjProjection> builder)
    {
        builder.ToTable("TestCustomContextSecurityObj", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name);
    }
}
