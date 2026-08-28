using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLocationCollectionPropertiesMap : IEntityTypeConfiguration<TestLocationCollectionProperties>
{
    public void Configure(EntityTypeBuilder<TestLocationCollectionProperties> builder)
    {
        builder.ToView(nameof(Location));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Location)).WithOne().HasForeignKey(typeof(TestLocationCollectionProperties), nameof(TestLocationCollectionProperties.Id));
        builder.Property<System.Guid?>("parentId_TestLocation").HasColumnName("parentId");
        builder.HasMany(x => x.Children).WithOne(x => x.Parent).HasForeignKey("parentId_TestLocation");
    }
}
