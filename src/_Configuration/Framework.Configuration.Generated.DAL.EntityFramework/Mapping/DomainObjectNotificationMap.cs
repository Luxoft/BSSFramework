using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainObjectNotificationMap : ConfigurationBaseMap<DomainObjectNotification>
{
    public override void Configure(EntityTypeBuilder<DomainObjectNotification> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.SerializeData).HasMaxLength(int.MaxValue).IsRequired();
    }
}
