using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestBusinessUnitTypeMap : IEntityTypeConfiguration<TestBusinessUnitType>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnitType> builder)
    {
        builder.ToTable("BusinessUnitType", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).IsRequired();
    }
}
