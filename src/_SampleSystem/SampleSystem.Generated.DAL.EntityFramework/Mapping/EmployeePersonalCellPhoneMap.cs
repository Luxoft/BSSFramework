using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeePersonalCellPhoneMap : IEntityTypeConfiguration<EmployeePersonalCellPhone>
{
    public void Configure(EntityTypeBuilder<EmployeePersonalCellPhone> builder)
    {
        builder.ToTable("EmployeePersonalCellPhone");
    }
}
