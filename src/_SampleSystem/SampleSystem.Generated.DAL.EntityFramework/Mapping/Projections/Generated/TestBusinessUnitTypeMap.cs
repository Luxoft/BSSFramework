using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestBusinessUnitTypeMap : IEntityTypeConfiguration<TestBusinessUnitType>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnitType> builder)
    {
        builder.ToView(nameof(BusinessUnitType));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnitType)).WithOne().HasForeignKey(typeof(TestBusinessUnitType), nameof(TestBusinessUnitType.Id));
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
    }
}
