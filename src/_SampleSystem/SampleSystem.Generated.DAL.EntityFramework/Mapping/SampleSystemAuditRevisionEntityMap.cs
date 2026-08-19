using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.AuditDomain;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SampleSystemAuditRevisionEntityMap : IEntityTypeConfiguration<SampleSystemAuditRevisionEntity>
{
    public void Configure(EntityTypeBuilder<SampleSystemAuditRevisionEntity> builder)
    {
        builder.ToTable("AuditRevisionEntity", "appAudit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Author).HasColumnName("author").IsRequired();
        builder.Property(x => x.RevisionDate).HasColumnName("RevisionDate").IsRequired();
    }
}
