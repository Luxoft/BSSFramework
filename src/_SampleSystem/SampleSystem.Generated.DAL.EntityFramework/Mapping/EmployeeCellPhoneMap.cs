using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeCellPhoneMap : IEntityTypeConfiguration<EmployeeCellPhone>
{
    public void Configure(EntityTypeBuilder<EmployeeCellPhone> builder)
    {
        builder.ToTable("EmployeeCellPhone");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Version).ValueGeneratedNever().IsConcurrencyToken().IsRequired();
        builder.Property(x => x.Active).IsRequired();
        builder.Property(x => x.CreateDate);
        builder.Property(x => x.CreatedBy);
        builder.Property(x => x.ModifiedBy);
        builder.Property(x => x.ModifyDate);
        builder.Property(x => x.CountryCode).HasMaxLength(3).IsRequired();
        builder.Property(x => x.CityCode).HasMaxLength(5).IsRequired();
        builder.Property(x => x.Number).HasMaxLength(7).IsRequired();
        builder.Property(x => x.FullNumber).HasMaxLength(18).IsRequired();

        builder.HasOne(x => x.Employee).WithMany(x => x.CellPhones).HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
