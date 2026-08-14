using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.ForUpdate;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class Example1Map : SampleSystemBaseMap<Example1>
{
    public override void Configure(EntityTypeBuilder<Example1> builder)
    {
        base.Configure(builder);
        builder.ToTable("Example1", "dbo");
        builder.Property(x => x.Field1);
        builder.Property(x => x.Field2);
        builder.Property(x => x.Field3);
        builder.HasMany(x => x.Items2).WithOne(x => x.Parent).HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
