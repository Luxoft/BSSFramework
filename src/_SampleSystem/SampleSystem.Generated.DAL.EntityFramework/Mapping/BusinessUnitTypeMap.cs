using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitTypeMap : SampleSystemBaseMap<BusinessUnitType>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitType> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameBusinessUnitType");
        builder.HasMany(x => x.PossibleFinancialProjectTypes).WithOne(x => x.BusinessUnitType).HasForeignKey("businessUnitTypeId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.PossibleParents).WithOne(x => x.BusinessUnitType).HasForeignKey("businessUnitTypeId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.TransferTo).WithOne(x => x.BusinessUnitType).HasForeignKey("businessUnitTypeId").OnDelete(DeleteBehavior.Cascade);
    }
}
