using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitIdentityMap : IEntityTypeConfiguration<BusinessUnitIdentity>
{
    public void Configure(EntityTypeBuilder<BusinessUnitIdentity> builder)
    {
        builder.ToTable("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(BusinessUnitIdentity), nameof(BusinessUnitIdentity.Id));
    }
}
