using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitToAncestorChildViewMap : SampleSystemBaseMap<BusinessUnitToAncestorChildView>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitToAncestorChildView> builder)
    {
        base.Configure(builder);
        builder.ToTable("BusinessUnitToAncestorChildView", table => table.ExcludeFromMigrations());
        builder.HasOne(x => x.ChildOrAncestor).WithMany().HasForeignKey("childOrAncestorId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Source).WithMany().HasForeignKey("sourceId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
