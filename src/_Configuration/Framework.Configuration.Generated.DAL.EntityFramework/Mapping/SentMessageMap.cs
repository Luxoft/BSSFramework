using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class SentMessageMap : ConfigurationBaseMap<SentMessage>
{
    public override void Configure(EntityTypeBuilder<SentMessage> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Copy).HasMaxLength(int.MaxValue);
        builder.Property(x => x.Message).HasMaxLength(int.MaxValue);
        builder.Property(x => x.ReplyTo).HasMaxLength(int.MaxValue);
        builder.Property(x => x.Subject).HasMaxLength(1000);
        builder.Property(x => x.To).HasMaxLength(int.MaxValue);
    }
}
