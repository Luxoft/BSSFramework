using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class SentMessageMap : ConfigurationBaseMap<SentMessage>
{
    public override void Configure(EntityTypeBuilder<SentMessage> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Copy).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(x => x.Message).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(x => x.ReplyTo).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(x => x.Subject).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.To).HasMaxLength(int.MaxValue).IsRequired();

        builder.Property(x => x.Comment).HasField("comment").UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired();
        builder.Property(x => x.ContextObjectId).HasField("contextObjectId").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.ContextObjectType).HasField("contextObjectType").UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired();
        builder.Property(x => x.From).HasField("from").UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired();
        builder.Property(x => x.TemplateName).HasField("templateName").UsePropertyAccessMode(PropertyAccessMode.Field).IsRequired();
    }
}
