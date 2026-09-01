using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class LocationToAncestorChildViewMap : SampleSystemBaseMap<LocationToAncestorChildView>
{
    public override void Configure(EntityTypeBuilder<LocationToAncestorChildView> builder)
    {
        base.Configure(builder);
        builder.ToTable(default(string?));
        builder.ToView(nameof(LocationToAncestorChildView));
        builder.HasOne(x => x.ChildOrAncestor).WithMany().HasForeignKey("childOrAncestorId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Source).WithMany().HasForeignKey("sourceId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
