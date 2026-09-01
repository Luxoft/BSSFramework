using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class LegalEntityBaseMap : SampleSystemBaseMap<LegalEntityBase>
{
    public override void Configure(EntityTypeBuilder<LegalEntityBase> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.NameEnglish).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameLegalEntityBase");
        builder.HasOne(x => x.BaseObj).WithMany().HasForeignKey("baseObjId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Addresses).WithOne(x => x.LegalEntity).HasForeignKey("legalEntityId").OnDelete(DeleteBehavior.Cascade);
        builder.ComplexProperty(x => x.AribaStatus, status => { status.Property(x => x.Date).HasColumnName("aribaStatusdate"); status.Property(x => x.Description).HasColumnName("aribaStatusdescription").HasMaxLength(int.MaxValue); status.Property(x => x.Type).HasColumnName("aribaStatustype"); });
    }
}
