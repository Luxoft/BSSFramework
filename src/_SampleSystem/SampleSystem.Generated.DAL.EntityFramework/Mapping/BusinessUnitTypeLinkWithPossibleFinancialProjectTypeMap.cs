using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitTypeLinkWithPossibleFinancialProjectTypeMap : SampleSystemBaseMap<BusinessUnitTypeLinkWithPossibleFinancialProjectType>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitTypeLinkWithPossibleFinancialProjectType> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.FinancialProjectType);
        builder.HasOne(x => x.BusinessUnitType).WithMany(x => x.PossibleFinancialProjectTypes).HasForeignKey("businessUnitTypeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
