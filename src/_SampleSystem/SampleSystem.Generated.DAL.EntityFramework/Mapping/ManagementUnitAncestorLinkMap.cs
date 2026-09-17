using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.MU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitAncestorLinkMap : SampleSystemBaseMap<ManagementUnitAncestorLink>
{
    public override void Configure(EntityTypeBuilder<ManagementUnitAncestorLink> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnitAncestorLink");
        builder.HasOne(x => x.Ancestor).WithMany().HasForeignKey("ancestorId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Child).WithMany().HasForeignKey("childId").OnDelete(DeleteBehavior.Restrict);
    }
}
