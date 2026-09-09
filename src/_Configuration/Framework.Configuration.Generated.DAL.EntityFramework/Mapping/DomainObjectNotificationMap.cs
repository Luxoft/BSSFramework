using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainObjectNotificationMap : ConfigurationBaseMap<DomainObjectNotification>
{
    public override void Configure(EntityTypeBuilder<DomainObjectNotification> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.SerializeData).HasMaxLength(int.MaxValue).IsRequired();

        builder.Property(x => x.Status).HasField("status").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.HostName).HasField("hostName").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.ProcessDate).HasField("processDate").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
