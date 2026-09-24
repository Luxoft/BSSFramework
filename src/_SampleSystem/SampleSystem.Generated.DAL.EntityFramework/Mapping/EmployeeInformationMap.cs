using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class EmployeeInformationMap : IEntityTypeConfiguration<EmployeeInformation>
{
    public void Configure(EntityTypeBuilder<EmployeeInformation> builder)
    {
        builder.ToTable("EmployeeInformation");
        builder.Property(x => x.PersonalEmail).HasMaxLength(50);
    }
}
