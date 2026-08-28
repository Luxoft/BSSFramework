using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SecurityEmployeeMap : IEntityTypeConfiguration<SecurityEmployee>
{
    public void Configure(EntityTypeBuilder<SecurityEmployee> builder)
    {
        builder.ToTable("Employee");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(SecurityEmployee), nameof(SecurityEmployee.Id));

        builder.Ignore(x => x.Login_Security);
        builder.Ignore(x => x.BusinessUnit_Security);
        builder.Ignore(x => x.Department_Security);
    }
}
