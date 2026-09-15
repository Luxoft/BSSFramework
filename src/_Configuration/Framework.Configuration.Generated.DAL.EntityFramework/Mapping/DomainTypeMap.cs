using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class DomainTypeMap : ConfigurationBaseMap<DomainType>
{
    public override void Configure(EntityTypeBuilder<DomainType> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();

        builder.HasOne(x => x.TargetSystem)
            .WithMany(x => x.DomainTypes)
            .HasForeignKey("TargetSystemId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder.HasIndex("Name", "Namespace", "TargetSystemId")
            .IsUnique()
            .HasDatabaseName("UIX_name_nameSpace_targetSystemDomainType");

        builder.HasMany(x => x.EventOperations)
            .WithOne(x => x.DomainType)
            .HasForeignKey("DomainTypeId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
    }
}
