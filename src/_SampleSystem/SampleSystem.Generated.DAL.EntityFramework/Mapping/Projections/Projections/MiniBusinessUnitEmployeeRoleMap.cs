using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class MiniBusinessUnitEmployeeRoleMap : IEntityTypeConfiguration<MiniBusinessUnitEmployeeRole>
{
    public void Configure(EntityTypeBuilder<MiniBusinessUnitEmployeeRole> builder)
    {
        builder.ToView(nameof(BusinessUnitEmployeeRole));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(BusinessUnitEmployeeRole)).WithOne().HasForeignKey(typeof(MiniBusinessUnitEmployeeRole), nameof(MiniBusinessUnitEmployeeRole.Id)).IsRequired();
        builder.Property<System.Guid?>("businessUnitId_MiniBusinessUnitEmployeeRole").HasColumnName("businessUnitId");
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId_MiniBusinessUnitEmployeeRole").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.Property<System.Guid?>("employeeId_MiniBusinessUnitEmployeeRole").HasColumnName("employeeId");
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId_MiniBusinessUnitEmployeeRole").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
    }
}
