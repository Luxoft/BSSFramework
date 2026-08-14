using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestEmployeeMap : IEntityTypeConfiguration<TestEmployee>
{
    public void Configure(EntityTypeBuilder<TestEmployee> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Login);
        builder.Property(x => x.NameEngFirstName);
        builder.HasOne(x => x.CoreBusinessUnit).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CoreBusinessUnit_Auto).WithMany().HasForeignKey("coreBusinessUnitId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Position_Auto).WithMany().HasForeignKey("positionId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Ppm_Auto).WithMany().HasForeignKey("ppmId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId").OnDelete(DeleteBehavior.Restrict);
    }
}
