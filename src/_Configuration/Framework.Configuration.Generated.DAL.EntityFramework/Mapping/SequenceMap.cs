using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class SequenceMap : ConfigurationBaseMap<Sequence>
{
    public override void Configure(EntityTypeBuilder<Sequence> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameSequence");
    }
}
