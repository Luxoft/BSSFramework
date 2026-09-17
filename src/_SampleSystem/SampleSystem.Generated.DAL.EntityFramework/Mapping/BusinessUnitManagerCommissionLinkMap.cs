using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitManagerCommissionLinkMap : SampleSystemBaseMap<BusinessUnitManagerCommissionLink>
{
    public override void Configure(EntityTypeBuilder<BusinessUnitManagerCommissionLink> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Commission).HasPrecision(19, 4).IsRequired();
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
        builder.HasOne(x => x.BusinessUnit).WithMany(x => x.ManagerCommissions).HasForeignKey("businessUnitId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Manager).WithMany().HasForeignKey("managerId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("businessUnitId", "managerId").IsUnique().HasDatabaseName("UIX_businessUnit_managerBusinessUnitManagerCommissionLink");
    }
}
