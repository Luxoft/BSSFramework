using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainObjectModificationMap : ConfigurationBaseMap<DomainObjectModification>
{
    public override void Configure(EntityTypeBuilder<DomainObjectModification> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.DomainObjectId).IsRequired();
        builder.Property(x => x.Revision).IsRequired();
        builder.Property(x => x.Version).IsRequired().ValueGeneratedNever().IsConcurrencyToken();

        builder.HasOne(x => x.DomainType)
            .WithMany()
            .HasForeignKey("DomainTypeId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder.HasIndex("DomainObjectId", "DomainTypeId", "Revision")
            .IsUnique()
            .HasDatabaseName("UIX_domainObjectId_domainType_revisionDomainObjectModification");
    }
}
