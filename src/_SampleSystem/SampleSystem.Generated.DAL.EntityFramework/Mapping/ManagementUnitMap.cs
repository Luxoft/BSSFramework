using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.MU;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitMap : SampleSystemBaseMap<ManagementUnit>
{
    public override void Configure(EntityTypeBuilder<ManagementUnit> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnit", "dbo");
        builder.Property(x => x.BusinessUnitStatus).IsRequired();
        builder.Property(x => x.DeepLevel).IsRequired();
        builder.Property(x => x.IsProduction).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.BusinessUnits).WithOne(x => x.ManagementUnit).HasForeignKey("managementUnitId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.HRDepartments).WithOne(x => x.ManagementUnit).HasForeignKey("managementUnitId").OnDelete(DeleteBehavior.Cascade);
    }
}
