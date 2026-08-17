using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class HRDepartmentMap : SampleSystemBaseMap<HRDepartment>
{
    public override void Configure(EntityTypeBuilder<HRDepartment> builder)
    {
        base.Configure(builder);
        builder.ToTable("HRDepartment", "dbo");
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.CodeNative).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ExternalId).IsRequired();
        builder.Property(x => x.IsLegal).IsRequired();
        builder.Property(x => x.IsProduction).IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.NameNative).IsRequired();
        builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("uni_code_HRDepartment");
        builder.HasIndex(x => x.CodeNative).IsUnique().HasDatabaseName("uni_codenative_HRDepartment");
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("uni_name_HRDepartment");
        builder.HasIndex(x => x.NameNative).IsUnique().HasDatabaseName("uni_namenative_HRDepartment");
        builder.HasOne(x => x.ApprovedBy).WithMany().HasForeignKey("approvedById").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.CompanyLegalEntity).WithMany().HasForeignKey("companyLegalEntityId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Head).WithMany().HasForeignKey("headId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.BusinessUnitHrDepartments).WithOne(x => x.HRDepartment).HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.EmployeePositions).WithOne(x => x.HrDepartment).HasForeignKey("hrDepartmentId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.HrDepartmentRoleEmployees).WithOne(x => x.HRDepartment).HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(x => x.ManagementUnits).WithOne(x => x.HRDepartment).HasForeignKey("hRDepartmentId").OnDelete(DeleteBehavior.Cascade);
    }
}
