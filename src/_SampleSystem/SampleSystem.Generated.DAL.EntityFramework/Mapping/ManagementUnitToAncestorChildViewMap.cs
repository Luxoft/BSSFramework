using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.MU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitToAncestorChildViewMap : SampleSystemBaseMap<ManagementUnitToAncestorChildView>
{
    public override void Configure(EntityTypeBuilder<ManagementUnitToAncestorChildView> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnitToAncestorChildView", "dbo", table => table.ExcludeFromMigrations());
        builder.HasOne(x => x.ChildOrAncestor).WithMany().HasForeignKey("childOrAncestorId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Source).WithMany().HasForeignKey("sourceId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
