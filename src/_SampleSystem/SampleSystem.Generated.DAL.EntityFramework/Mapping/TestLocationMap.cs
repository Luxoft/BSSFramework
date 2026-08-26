using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLocationMap : IEntityTypeConfiguration<TestLocation>
{
    public void Configure(EntityTypeBuilder<TestLocation> builder)
    {
        builder.ToTable("Location", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Location)).WithOne().HasForeignKey(typeof(TestLocation), nameof(TestLocation.Id));
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
