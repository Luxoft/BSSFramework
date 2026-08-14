using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestSecurityObjItemProjectionMap : IEntityTypeConfiguration<TestSecurityObjItemProjection>
{
    public void Configure(EntityTypeBuilder<TestSecurityObjItemProjection> builder)
    {
        builder.ToTable("TestSecurityObjItem", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name);
    }
}
