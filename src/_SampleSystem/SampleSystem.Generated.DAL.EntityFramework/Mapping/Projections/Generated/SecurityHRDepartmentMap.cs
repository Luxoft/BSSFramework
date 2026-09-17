using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class SecurityHRDepartmentMap : IEntityTypeConfiguration<SecurityHRDepartment>
{
    public void Configure(EntityTypeBuilder<SecurityHRDepartment> builder)
    {
        builder.ToView(nameof(SecurityHRDepartment));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("locationId_SecurityHRDepartment").HasColumnName("locationId");
        builder.HasOne(x => x.Location_Security).WithMany().HasForeignKey("locationId_SecurityHRDepartment");
    }
}
