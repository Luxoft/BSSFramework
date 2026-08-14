using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitMap : SampleSystemBaseMap<BusinessUnit>
{
    public override void Configure(EntityTypeBuilder<BusinessUnit> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name);
        builder.Property(x => x.BusinessUnitStatus);
        builder.Property(x => x.Commission).HasPrecision(19, 4);
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
        builder.HasOne(x => x.BusinessUnitForRent).WithMany().HasForeignKey("businessUnitForRentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.BusinessUnitType).WithMany().HasForeignKey("businessUnitTypeId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.BusinessUnitEmployeeRoles).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ManagerCommissions).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Cascade);
    }
}
