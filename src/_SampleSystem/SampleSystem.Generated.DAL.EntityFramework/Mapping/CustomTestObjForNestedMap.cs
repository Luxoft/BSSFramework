using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class CustomTestObjForNestedMap : IEntityTypeConfiguration<CustomTestObjForNested>
{
    public void Configure(EntityTypeBuilder<CustomTestObjForNested> builder)
    {
        builder.ToTable("TestObjForNestedBase");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(TestObjForNestedBase)).WithOne().HasForeignKey(typeof(CustomTestObjForNested), nameof(CustomTestObjForNested.Id));
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.PeriodStartDateXXX).HasColumnName("periodstartDate");
    }
}
