using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class SecurityLocationMap : IEntityTypeConfiguration<SecurityLocation>
{
    public void Configure(EntityTypeBuilder<SecurityLocation> builder)
    {
        builder.ToView(nameof(SecurityLocation));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
