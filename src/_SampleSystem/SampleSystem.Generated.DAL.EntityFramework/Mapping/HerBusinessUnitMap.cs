using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class HerBusinessUnitMap : IEntityTypeConfiguration<HerBusinessUnit>
{
    public void Configure(EntityTypeBuilder<HerBusinessUnit> builder)
    {
        builder.ToTable("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(HerBusinessUnit), nameof(HerBusinessUnit.Id));
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
