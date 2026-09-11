using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class BusinessUnitProgramClassAutoPropBusinessUnitTypeMap : IEntityTypeConfiguration<BusinessUnitProgramClass_AutoProp_BusinessUnitType>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass_AutoProp_BusinessUnitType> builder)
    {
        builder.ToView(nameof(BusinessUnitProgramClass_AutoProp_BusinessUnitType));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
