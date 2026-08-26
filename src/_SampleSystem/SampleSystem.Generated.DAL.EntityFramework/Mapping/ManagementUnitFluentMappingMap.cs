using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.NhFluentMapping;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ManagementUnitFluentMappingMap : SampleSystemBaseMap<ManagementUnitFluentMapping>
{
    public override void Configure(EntityTypeBuilder<ManagementUnitFluentMapping> builder)
    {
        base.Configure(builder);
        builder.ToTable("ManagementUnitFluentMapping");
        builder.Property(x => x.IsProduction).IsRequired();
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.OwnsOne(x => x.MuComponent, muComponent =>
        {
            muComponent.Property(x => x.LuxoftSignsFirst).HasColumnName("muComponentluxoftSignsFirst");
            muComponent.HasOne(x => x.AuthorizedLuxoftSignatory).WithMany().HasForeignKey("muComponentauthorizedLuxoftSignatoryId").OnDelete(DeleteBehavior.Restrict);
        });
    }
}
