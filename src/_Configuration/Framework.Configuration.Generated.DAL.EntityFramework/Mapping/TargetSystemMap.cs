using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class TargetSystemMap : ConfigurationBaseMap<TargetSystem>
{
    public override void Configure(EntityTypeBuilder<TargetSystem> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.IsBase).IsRequired();
        builder.Property(x => x.IsMain).IsRequired();
        builder.Property(x => x.IsRevision).IsRequired();
        builder.Property(x => x.SubscriptionEnabled).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameTargetSystem");

        builder.HasMany(x => x.DomainTypes)
            .WithOne(x => x.TargetSystem)
            .HasForeignKey("TargetSystemId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
    }
}
