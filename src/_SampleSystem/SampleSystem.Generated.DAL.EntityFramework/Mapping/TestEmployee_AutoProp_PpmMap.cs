using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeAutoPropPpmMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Ppm>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Ppm> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.NameNativeMiddleName_Last_PpmNameNativeMiddleName).IsRequired();
    }
}
