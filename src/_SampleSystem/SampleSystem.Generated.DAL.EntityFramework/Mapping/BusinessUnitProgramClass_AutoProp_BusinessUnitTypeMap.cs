using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitProgramClassAutoPropBusinessUnitTypeMap : IEntityTypeConfiguration<BusinessUnitProgramClass_AutoProp_BusinessUnitType>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass_AutoProp_BusinessUnitType> builder)
    {
        builder.ToTable("BusinessUnitType");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnitType)).WithOne().HasForeignKey(typeof(BusinessUnitProgramClass_AutoProp_BusinessUnitType), nameof(BusinessUnitProgramClass_AutoProp_BusinessUnitType.Id));
    }
}
