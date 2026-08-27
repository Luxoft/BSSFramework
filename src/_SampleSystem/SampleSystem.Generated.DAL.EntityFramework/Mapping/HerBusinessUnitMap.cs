using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        ((IConventionForeignKey)builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(HerBusinessUnit), nameof(HerBusinessUnit.Id)).IsRequired().Metadata).SetIsRequiredDependent(true);
        var nameProperty = builder.Property(x => x.Name).HasColumnName("Name").IsRequired().Metadata;
        nameProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        nameProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        var parentIdProperty = builder.Property<System.Guid?>("parentId_HerBusinessUnit").HasColumnName("parentId").Metadata;
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId_HerBusinessUnit").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("parentId_HerBusinessUnit").HasDatabaseName("IX_BusinessUnit_parentId_HerBusinessUnit");
        parentIdProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        parentIdProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
