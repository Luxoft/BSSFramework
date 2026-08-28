using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeAutoPropPpmMap : IEntityTypeConfiguration<TestEmployee_AutoProp_Ppm>
{
    public void Configure(EntityTypeBuilder<TestEmployee_AutoProp_Ppm> builder)
    {
        builder.ToView("Employee");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestEmployee_AutoProp_Ppm), nameof(TestEmployee_AutoProp_Ppm.Id));
        builder.Property(x => x.NameNativeMiddleName_Last_PpmNameNativeMiddleName).HasColumnName("nameNativemiddleName");
    }
}
