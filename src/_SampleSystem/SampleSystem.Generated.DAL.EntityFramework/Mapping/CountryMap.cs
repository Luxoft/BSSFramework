using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class CountryMap : SampleSystemBaseMap<Country>
{
    public override void Configure(EntityTypeBuilder<Country> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.Culture).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.NameNative).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameCountry");
    }
}
