using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLocationCollectionPropertiesMap : IEntityTypeConfiguration<TestLocationCollectionProperties>
{
    public void Configure(EntityTypeBuilder<TestLocationCollectionProperties> builder)
    {
        builder.ToTable("Location", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.HasMany(x => x.Children).WithOne(x => x.Parent).HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
