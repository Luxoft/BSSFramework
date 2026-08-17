using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.NLock;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class GenericNamedLockMap : SampleSystemBaseMap<GenericNamedLock>
{
    public override void Configure(EntityTypeBuilder<GenericNamedLock> builder)
    {
        base.Configure(builder);
        builder.ToTable("GenericNamedLock", "dbo");
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameGenericNamedLock");
    }
}
