using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SecurityLocationMap : IEntityTypeConfiguration<SecurityLocation>
{
    public void Configure(EntityTypeBuilder<SecurityLocation> builder)
    {
        builder.ToView(nameof(Location));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Location)).WithOne().HasForeignKey(typeof(SecurityLocation), nameof(SecurityLocation.Id));
    }
}
