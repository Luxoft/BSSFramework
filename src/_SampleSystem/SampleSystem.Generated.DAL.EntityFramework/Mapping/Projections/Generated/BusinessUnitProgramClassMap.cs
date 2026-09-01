using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class BusinessUnitProgramClassMap : IEntityTypeConfiguration<BusinessUnitProgramClass>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass> builder)
    {
        builder.ToView(nameof(BusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnit)).WithOne().HasForeignKey(typeof(BusinessUnitProgramClass), nameof(BusinessUnitProgramClass.Id)).IsRequired();
        builder.Property(x => x.IsNewBusiness).HasColumnName("IsNewBusiness").IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.PeriodEndDate).HasColumnName("periodendDate").IsRequired();
        builder.Property<System.Guid?>("businessUnitTypeId_BusinessUnitProgramClass").HasColumnName("businessUnitTypeId");
        builder.HasOne(x => x.BusinessUnitType_Auto).WithMany().HasForeignKey("businessUnitTypeId_BusinessUnitProgramClass").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("businessUnitTypeId_BusinessUnitProgramClass").HasDatabaseName("IX_BusinessUnit_businessUnitTypeId_BusinessUnitProgramClass");
    }
}
