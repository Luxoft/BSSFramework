using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class LocationMap : SampleSystemBaseMap<Location>
{
    public override void Configure(EntityTypeBuilder<Location> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.BinaryData).HasMaxLength(int.MaxValue);
        builder.Property(x => x.CloseDate).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameLocation");
        builder.HasOne(x => x.Country).WithMany().HasForeignKey("countryId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
