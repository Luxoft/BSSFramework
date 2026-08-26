using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeCellPhoneBaseMap : SampleSystemBaseMap<EmployeeCellPhoneBase>
{
    public override void Configure(EntityTypeBuilder<EmployeeCellPhoneBase> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.CountryCode).HasMaxLength(3).IsRequired();
        builder.Property(x => x.CityCode).HasMaxLength(5).IsRequired();
        builder.Property(x => x.Number).HasMaxLength(7).IsRequired();
        builder.Property(x => x.FullNumber).HasMaxLength(18).IsRequired();
    }
}
