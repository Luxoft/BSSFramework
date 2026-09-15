using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainObjectEventMap : ConfigurationBaseMap<DomainObjectEvent>
{
    public override void Configure(EntityTypeBuilder<DomainObjectEvent> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.QueueTag).IsRequired();
        builder.Property(x => x.SerializeData).HasMaxLength(int.MaxValue).IsRequired();
        builder.Property(x => x.SerializeType).HasMaxLength(int.MaxValue).IsRequired();

        builder.Property(x => x.Status).HasField("status").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.HostName).HasField("hostName").UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Property(x => x.ProcessDate).HasField("processDate").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(x => x.Operation)
            .WithMany()
            .HasForeignKey("OperationId")
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
    }
}
