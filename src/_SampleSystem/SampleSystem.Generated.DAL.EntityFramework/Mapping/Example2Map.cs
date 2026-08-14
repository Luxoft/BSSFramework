using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.ForUpdate;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class Example2Map : SampleSystemBaseMap<Example2>
{
    public override void Configure(EntityTypeBuilder<Example2> builder)
    {
        base.Configure(builder);
        builder.ToTable("Example2", "dbo");
        builder.Property(x => x.Field1);
        builder.Property(x => x.Field2);
        builder.HasOne(x => x.Parent).WithMany(x => x.Items2).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
