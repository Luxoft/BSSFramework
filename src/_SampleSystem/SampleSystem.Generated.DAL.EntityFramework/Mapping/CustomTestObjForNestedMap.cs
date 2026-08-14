using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class CustomTestObjForNestedMap : IEntityTypeConfiguration<CustomTestObjForNested>
{
    public void Configure(EntityTypeBuilder<CustomTestObjForNested> builder)
    {
        builder.ToTable("TestObjForNested", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.SplitToTable("TestObjForNestedBase", split =>
        {
            split.Property(x => x.Name).HasColumnName("name");
            split.Property(x => x.PeriodStartDateXXX).HasColumnName("periodStartDate");
        });
    }
}
