using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestLocationCollectionPropertiesMap : IEntityTypeConfiguration<TestLocationCollectionProperties>
{
    public void Configure(EntityTypeBuilder<TestLocationCollectionProperties> builder)
    {
        builder.ToView(nameof(TestLocationCollectionProperties));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasMany(x => x.Children).WithOne(x => x.Parent).HasForeignKey("parentId_TestLocation");
    }
}
