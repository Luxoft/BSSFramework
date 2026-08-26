using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.MU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitAndBusinessUnitLinkMap : SampleSystemBaseMap<ManagementUnitAndBusinessUnitLink>
{
    public override void Configure(EntityTypeBuilder<ManagementUnitAndBusinessUnitLink> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnitAndBusinessUnitLink", "dbo");
        builder.Property(x => x.EqualBU).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany(x => x.ManagementUnits).HasForeignKey("businessUnitId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.ManagementUnit).WithMany(x => x.BusinessUnits).HasForeignKey("managementUnitId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("businessUnitId", "managementUnitId").IsUnique().HasDatabaseName("UIX_businessUnit_managementUnitManagementUnitAndBusinessUnitLink");
    }
}
