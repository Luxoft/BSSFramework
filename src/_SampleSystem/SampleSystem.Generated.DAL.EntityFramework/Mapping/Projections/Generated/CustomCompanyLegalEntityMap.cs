using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class CustomCompanyLegalEntityMap : IEntityTypeConfiguration<CustomCompanyLegalEntity>
{
    public void Configure(EntityTypeBuilder<CustomCompanyLegalEntity> builder)
    {
        builder.ToView(nameof(CustomCompanyLegalEntity));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.NameEnglish).HasColumnName("NameEnglish").IsRequired();
        builder.Property(x => x.AribaStatusDescription).HasColumnName("aribaStatusdescription");
        builder.Property(x => x.AribaStatusType).HasColumnName("aribaStatustype");
        builder.Property<System.Guid?>("baseObjId").HasColumnName("baseObjId");
        builder.HasOne(x => x.BaseObj).WithMany().HasForeignKey("baseObjId");
        builder.Property<System.Guid?>("currentObjId").HasColumnName("currentObjId");
        builder.HasOne(x => x.CurrentObj).WithMany().HasForeignKey("currentObjId");
    }
}
