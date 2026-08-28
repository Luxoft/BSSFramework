using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitProgramClassMap : IEntityTypeConfiguration<BusinessUnitProgramClass>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass> builder)
    {
        builder.ToView("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(BusinessUnitProgramClass), nameof(BusinessUnitProgramClass.Id)).IsRequired();
        var isNewBusinessProperty = builder.Property(x => x.IsNewBusiness).HasColumnName("IsNewBusiness").IsRequired().Metadata;
        isNewBusinessProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        isNewBusinessProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        var nameProperty = builder.Property(x => x.Name).HasColumnName("Name").IsRequired().Metadata;
        nameProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        nameProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        builder.Property(x => x.PeriodEndDate).HasColumnName("periodendDate").IsRequired();
        var businessUnitTypeIdProperty = builder.Property<System.Guid?>("businessUnitTypeId_BusinessUnitProgramClass").HasColumnName("businessUnitTypeId").Metadata;
        builder.HasOne(x => x.BusinessUnitType_Auto).WithMany().HasForeignKey("businessUnitTypeId_BusinessUnitProgramClass").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("businessUnitTypeId_BusinessUnitProgramClass").HasDatabaseName("IX_BusinessUnit_businessUnitTypeId_BusinessUnitProgramClass");
        businessUnitTypeIdProperty.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        businessUnitTypeIdProperty.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
