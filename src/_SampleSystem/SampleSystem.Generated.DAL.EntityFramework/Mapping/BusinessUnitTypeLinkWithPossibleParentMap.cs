using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitTypeLinkWithPossibleParentMap : SampleSystemBaseMap<BusinessUnitTypeLinkWithPossibleParent>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitTypeLinkWithPossibleParent> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.BusinessUnitType).WithMany(x => x.PossibleParents).HasForeignKey("businessUnitTypeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.PossibleParent).WithMany().HasForeignKey("possibleParentId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
