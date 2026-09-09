using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestEmployeeAutoPropPpmMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Ppm>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Ppm> builder)
    {
        builder.ToView(nameof(TestEmployee_AutoProp_Ppm));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.NameNativeMiddleName_Last_PpmNameNativeMiddleName).HasColumnName("nameNativemiddleName");
    }
}
