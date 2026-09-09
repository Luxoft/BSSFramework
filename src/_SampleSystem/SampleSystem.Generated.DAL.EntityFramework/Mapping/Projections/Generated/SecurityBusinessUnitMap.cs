using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class SecurityBusinessUnitMap : IEntityTypeConfiguration<SecurityBusinessUnit>
{
    public void Configure(EntityTypeBuilder<SecurityBusinessUnit> builder)
    {
        builder.ToView(nameof(SecurityBusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
