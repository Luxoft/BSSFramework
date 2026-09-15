using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitTypeLinkWithTransferToMap : SampleSystemBaseMap<BusinessUnitTypeLinkWithTransferTo>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitTypeLinkWithTransferTo> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.BusinessUnitType).WithMany(x => x.TransferTo).HasForeignKey("businessUnitTypeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.TransferTo).WithMany().HasForeignKey("transferToId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
