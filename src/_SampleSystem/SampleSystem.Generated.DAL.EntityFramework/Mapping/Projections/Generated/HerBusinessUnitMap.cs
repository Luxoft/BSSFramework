using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class HerBusinessUnitMap : IEntityTypeConfiguration<HerBusinessUnit>
{
    public void Configure(EntityTypeBuilder<HerBusinessUnit> builder)
    {
        builder.ToView(nameof(BusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        ((IConventionForeignKey)builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(HerBusinessUnit), nameof(HerBusinessUnit.Id)).IsRequired().Metadata).SetIsRequiredDependent(true);
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property<System.Guid?>("parentId_HerBusinessUnit").HasColumnName("parentId");
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId_HerBusinessUnit").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("parentId_HerBusinessUnit").HasDatabaseName("IX_BusinessUnit_parentId_HerBusinessUnit");
    }
}
