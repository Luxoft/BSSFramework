using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class CustomTestObjForNestedMap : IEntityTypeConfiguration<CustomTestObjForNested>
{
    public void Configure(EntityTypeBuilder<CustomTestObjForNested> builder)
    {
        builder.ToView(nameof(CustomTestObjForNested));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.PeriodStartDateXXX).HasColumnName("periodstartDate");
    }
}
