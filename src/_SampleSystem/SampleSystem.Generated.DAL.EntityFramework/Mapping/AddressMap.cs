using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class AddressMap : SampleSystemBaseMap<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.AddressType);
        builder.Property(x => x.CityName).HasMaxLength(100);
        builder.Property(x => x.RegionName).HasMaxLength(100);
        builder.Property(x => x.Street).HasMaxLength(100);
        builder.Property(x => x.Zip).HasMaxLength(100);
        builder.HasOne(x => x.CountryName).WithMany().HasForeignKey("countryNameId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.LegalEntity).WithMany().HasForeignKey("legalEntityId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
