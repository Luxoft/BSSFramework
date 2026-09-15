using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestEmployeeMap : IEntityTypeConfiguration<TestEmployee>
{
    public void Configure(EntityTypeBuilder<TestEmployee> builder)
    {
        builder.ToView(nameof(TestEmployee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.NameEngFirstName).HasColumnName("nameEngfirstName").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Login).HasColumnName("Login").HasMaxLength(30).IsRequired();
        builder.Property<System.Guid?>("coreBusinessUnitId_TestEmployee").HasColumnName("coreBusinessUnitId");
        builder.HasOne(x => x.CoreBusinessUnit).WithMany().HasForeignKey("coreBusinessUnitId_TestEmployee");
        builder.HasOne(x => x.CoreBusinessUnit_Auto).WithMany().HasForeignKey("coreBusinessUnitId_TestEmployee");
        builder.Property<System.Guid?>("positionId_TestEmployee").HasColumnName("positionId");
        builder.HasOne(x => x.Position_Auto).WithMany().HasForeignKey("positionId_TestEmployee");
        builder.Property<System.Guid?>("ppmId_TestEmployee").HasColumnName("ppmId");
        builder.HasOne(x => x.Ppm_Auto).WithMany().HasForeignKey("ppmId_TestEmployee");
        builder.Property<System.Guid?>("roleId_TestEmployee").HasColumnName("roleId");
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId_TestEmployee");
    }
}
