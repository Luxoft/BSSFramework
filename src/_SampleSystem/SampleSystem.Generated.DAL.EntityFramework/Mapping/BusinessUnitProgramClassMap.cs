using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitProgramClassMap : IEntityTypeConfiguration<BusinessUnitProgramClass>
{
    public void Configure(EntityTypeBuilder<BusinessUnitProgramClass> builder)
    {
        builder.ToTable("BusinessUnit", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.IsNewBusiness);
        builder.Property(x => x.Name);
        builder.Property(x => x.PeriodEndDate);
        builder.HasOne(x => x.BusinessUnitType_Auto).WithMany().HasForeignKey("businessUnitTypeId").OnDelete(DeleteBehavior.Restrict);
    }
}
