using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class SecurityHRDepartmentMap : IEntityTypeConfiguration<SecurityHRDepartment>
{
    public void Configure(EntityTypeBuilder<SecurityHRDepartment> builder)
    {
        builder.ToView(nameof(HRDepartment));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(HRDepartment)).WithOne().HasForeignKey(typeof(SecurityHRDepartment), nameof(SecurityHRDepartment.Id));
        builder.Property<System.Guid?>("locationId_SecurityHRDepartment").HasColumnName("locationId");
        builder.HasOne(x => x.Location_Security).WithMany().HasForeignKey("locationId_SecurityHRDepartment");
    }
}
