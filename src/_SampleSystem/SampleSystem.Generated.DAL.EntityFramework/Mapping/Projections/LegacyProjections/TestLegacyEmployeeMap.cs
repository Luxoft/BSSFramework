using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLegacyEmployeeMap : IEntityTypeConfiguration<TestLegacyEmployee>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee> builder)
    {
        builder.HasBaseType((Type)null);
        builder.ToView(nameof(Employee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Employee)).WithOne().HasForeignKey(typeof(TestLegacyEmployee), nameof(TestLegacyEmployee.Id));
        builder.Property<System.Guid?>("roleId_TestLegacyEmployee").HasColumnName("roleId");
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId_TestLegacyEmployee");
    }
}
