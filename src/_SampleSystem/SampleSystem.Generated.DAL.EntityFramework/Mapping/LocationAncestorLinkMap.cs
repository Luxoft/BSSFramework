using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class LocationAncestorLinkMap : SampleSystemBaseMap<LocationAncestorLink>
{
    public override void Configure(EntityTypeBuilder<LocationAncestorLink> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.Ancestor).WithMany().HasForeignKey("ancestorId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Child).WithMany().HasForeignKey("childId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
