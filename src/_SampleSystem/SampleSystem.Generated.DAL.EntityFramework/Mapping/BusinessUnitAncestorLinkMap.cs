using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitAncestorLinkMap : SampleSystemBaseMap<BusinessUnitAncestorLink>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitAncestorLink> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.Ancestor).WithMany().HasForeignKey("ancestorId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Child).WithMany().HasForeignKey("childId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
