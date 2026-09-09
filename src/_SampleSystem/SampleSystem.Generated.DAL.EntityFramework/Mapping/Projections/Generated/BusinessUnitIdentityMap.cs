using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class BusinessUnitIdentityMap : IEntityTypeConfiguration<BusinessUnitIdentity>
{
    public void Configure(EntityTypeBuilder<BusinessUnitIdentity> builder)
    {
        builder.ToView(nameof(BusinessUnitIdentity));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
