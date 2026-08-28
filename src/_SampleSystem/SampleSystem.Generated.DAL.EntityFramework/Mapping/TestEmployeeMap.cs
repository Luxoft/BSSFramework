using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeMap : IEntityTypeConfiguration<TestEmployee>
{
    public void Configure(EntityTypeBuilder<TestEmployee> builder)
    {
        builder.ToTable("Employee");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestEmployee), nameof(TestEmployee.Id));
        builder.Ignore(x => x.Login);
        builder.Property(x => x.NameEngFirstName).HasColumnName("nameEngfirstName").HasMaxLength(50).IsRequired();
        builder.Ignore(x => x.CoreBusinessUnit_Auto);
        builder.Ignore(x => x.Position_Auto);
        builder.Ignore(x => x.Ppm_Auto);
        builder.Ignore(x => x.Role_Auto);
    }
}
