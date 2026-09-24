using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.LegacyProjections;

public class TestLegacyEmployeeMap : IEntityTypeConfiguration<TestLegacyEmployee>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee> builder)
    {
        builder.HasBaseType((Type)null);
        builder.ToView(nameof(TestLegacyEmployee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Login).HasColumnName("login");
        builder.Property<System.Guid?>("coreBusinessUnitId_TestLegacyEmployee").HasColumnName("coreBusinessUnitId");
        builder.HasOne(x => x.BusinessUnit_Security).WithMany().HasForeignKey("coreBusinessUnitId_TestLegacyEmployee");
        builder.Property<System.Guid?>("hRDepartmentId_TestLegacyEmployee").HasColumnName("hRDepartmentId");
        builder.HasOne(x => x.Department_Security).WithMany().HasForeignKey("hRDepartmentId_TestLegacyEmployee");
        builder.Property<System.Guid?>("roleId_TestLegacyEmployee").HasColumnName("roleId");
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId_TestLegacyEmployee");
    }
}
