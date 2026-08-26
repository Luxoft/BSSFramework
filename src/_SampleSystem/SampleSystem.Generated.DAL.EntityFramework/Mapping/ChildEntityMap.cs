using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.UniqueByParent;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ChildEntityMap : SampleSystemBaseMap<ChildEntity>
{
    public override void Configure(EntityTypeBuilder<ChildEntity> builder)
    {
        base.Configure(builder);
        builder.ToTable("ChildEntity");
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("parentId").IsUnique().HasDatabaseName("parent_ChildEntity");
    }
}
