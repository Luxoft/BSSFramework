using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class InsuranceDetailMap : SampleSystemBaseMap<InsuranceDetail>
{
    public override void Configure(EntityTypeBuilder<InsuranceDetail> builder)
    {
        base.Configure(builder);
        builder.ToTable("InsuranceDetail", "dbo");
        builder.Property(x => x.Cost).HasPrecision(19, 4).IsRequired();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.BirthDate);
        builder.Property(x => x.CellPhone);
        builder.Property(x => x.LandlinePhone);
        builder.Property(x => x.RegistrationAddress);
        builder.Property(x => x.ResidentalAddress);
        builder.ComplexProperty(x => x.Fio, fio =>
        {
            fio.Property(x => x.FirstName).HasColumnName("fiofirstName").HasMaxLength(50);
            fio.Property(x => x.LastName).HasColumnName("fiolastName").HasMaxLength(50);
            fio.Property(x => x.MiddleName).HasColumnName("fiomiddleName").HasMaxLength(50);
        });
    }
}
