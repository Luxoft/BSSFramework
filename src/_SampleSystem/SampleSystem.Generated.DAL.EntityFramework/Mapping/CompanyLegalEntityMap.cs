using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Directories;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class CompanyLegalEntityMap : IEntityTypeConfiguration<CompanyLegalEntity>
{
    public void Configure(EntityTypeBuilder<CompanyLegalEntity> builder)
    {
        builder.ToTable("CompanyLegalEntity");
        builder.HasOne(typeof(LegalEntityBase)).WithOne().HasForeignKey(typeof(CompanyLegalEntity), nameof(CompanyLegalEntity.Id)).OnDelete(DeleteBehavior.ClientCascade);
        builder.Property(x => x.Code).HasColumnName("Code").HasMaxLength(100).IsRequired();
        builder.Property<System.Guid?>("currentObjId").HasColumnName("currentObjId");
        builder.HasOne(x => x.CurrentObj).WithMany().HasForeignKey("currentObjId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
