using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeCellPhoneMap : IEntityTypeConfiguration<EmployeeCellPhone>
{
    public void Configure(EntityTypeBuilder<EmployeeCellPhone> builder)
    {
        builder.ToTable("EmployeeCellPhone");
        builder.HasOne(x => x.Employee).WithMany(x => x.CellPhones).HasForeignKey("employeeId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
