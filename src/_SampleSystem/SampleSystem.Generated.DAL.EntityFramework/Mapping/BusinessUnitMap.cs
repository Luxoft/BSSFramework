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
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.IsNewBusiness).HasColumnName("IsNewBusiness").IsRequired();
        builder.Property(x => x.BusinessUnitStatus).IsRequired();
        builder.Property(x => x.Commission).HasPrecision(19, 4).IsRequired();
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
        builder.HasOne(x => x.BusinessUnitForRent).WithMany().HasForeignKey("businessUnitForRentId").OnDelete(DeleteBehavior.Restrict);
        builder.Property<System.Guid?>("businessUnitTypeId").HasColumnName("businessUnitTypeId");
        builder.HasOne(x => x.BusinessUnitType).WithMany().HasForeignKey("businessUnitTypeId").HasConstraintName("FK_BusinessUnit_businessUnitTypeId_BusinessUnitType").OnDelete(DeleteBehavior.Restrict);
        builder.Property<System.Guid?>("parentId").HasColumnName("parentId");
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").HasConstraintName("FK_BusinessUnit_parentId_BusinessUnit").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.BusinessUnitEmployeeRoles).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ManagerCommissions).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Cascade);
    }
}
