using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitProgramClassAutoPropBusinessUnitTypeMap : IEntityTypeConfiguration<BusinessUnitProgramClass_AutoProp_BusinessUnitType>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass_AutoProp_BusinessUnitType> builder)
    {
        builder.ToTable("BusinessUnitType", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
    }
}
