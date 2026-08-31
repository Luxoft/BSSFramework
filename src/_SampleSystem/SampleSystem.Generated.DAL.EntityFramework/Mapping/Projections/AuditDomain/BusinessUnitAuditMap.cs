using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.AuditDomain;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.AuditDomain;

public class BusinessUnitAuditMap : IEntityTypeConfiguration<BusinessUnitAudit>
{
    public void Configure(EntityTypeBuilder<BusinessUnitAudit> builder)
    {
        builder.ToView(nameof(BusinessUnitAudit), "appAudit");
        builder.ComplexProperty(
            x => x.Identifier,
            identifier =>
            {
                identifier.Property(x => x.Id).HasColumnName("Id");
                identifier.Property(x => x.RevNumber).HasColumnName("REV");
            });
        builder.Property(x => x.Id).HasColumnName("Id").ValueGeneratedNever().IsRequired();
        builder.Property<long>("REV").HasColumnName("REV").ValueGeneratedNever().IsRequired();
        builder.HasKey(nameof(BusinessUnitAudit.Id), "REV");
        builder.HasOne(x => x.Revision).WithMany().HasForeignKey("REV").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy").IsRequired();
        builder.Property(x => x.ModifyDate).HasColumnName("ModifyDate");
        builder.Property(x => x.RevType).HasColumnName("REVTYPE").IsRequired();
    }
}
