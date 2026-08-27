using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestBusinessUnitMap : IEntityTypeConfiguration<TestBusinessUnit>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnit> builder)
    {
        builder.ToTable("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        var nameProperty = builder.Property(x => x.Name).HasColumnName("Name").IsRequired().Metadata;
        nameProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        nameProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(x => x.PeriodEndDate).HasColumnName("periodendDate").IsRequired();
        builder.HasOne(x => x.Her).WithMany().HasForeignKey("Id").IsRequired().OnDelete(DeleteBehavior.Restrict);
        var parentIdProperty = builder.Property<System.Guid?>("parentId_TestBusinessUnit").HasColumnName("parentId").Metadata;
        builder.HasOne(x => x.Parent_Auto).WithMany().HasForeignKey("parentId_TestBusinessUnit").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("parentId_TestBusinessUnit").HasDatabaseName("IX_BusinessUnit_parentId_TestBusinessUnit");
        parentIdProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        parentIdProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.HasMany(x => x.BusinessUnitEmployeeRoles).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.ClientCascade);
        builder.Ignore(x => x.CalcProp);
    }
}
