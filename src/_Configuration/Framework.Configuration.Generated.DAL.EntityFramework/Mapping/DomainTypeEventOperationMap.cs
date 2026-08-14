using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainTypeEventOperationMap : ConfigurationBaseMap<DomainTypeEventOperation>
{
    public override void Configure(EntityTypeBuilder<DomainTypeEventOperation> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.DomainType)
            .WithMany(x => x.EventOperations)
            .HasForeignKey("DomainTypeId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
        builder.HasIndex("DomainTypeId", "Name")
            .IsUnique()
            .HasDatabaseName("UIX_domainType_nameDomainTypeEventOperation");
    }
}
