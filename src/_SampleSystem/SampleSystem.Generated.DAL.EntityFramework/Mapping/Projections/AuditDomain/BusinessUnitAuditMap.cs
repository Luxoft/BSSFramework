using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.AuditDomain;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class BusinessUnitAuditMap : IEntityTypeConfiguration<BusinessUnitAudit>
{
    public void Configure(EntityTypeBuilder<BusinessUnitAudit> builder)
    {
        builder.ToView("BusinessUnitAudit", "appAudit");
        builder.Ignore(x => x.Identifier);
        builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
        builder.Property<long>("REV").HasColumnName("REV").ValueGeneratedNever().IsRequired();
        builder.HasKey(nameof(BusinessUnitAudit.Id), "REV");
        builder.HasOne(x => x.Revision).WithMany().HasForeignKey("REV").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsRequired();
        builder.Property(x => x.ModifyDate).HasColumnName("ModifyDate");
        builder.Property(x => x.RevType).HasColumnName("REVTYPE").IsRequired();
    }
}
