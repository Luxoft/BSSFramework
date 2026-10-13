using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestIdComment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCommentParentMap : SampleSystemBaseMap<TestCommentParent>
{
    public override void Configure(EntityTypeBuilder<TestCommentParent> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestCommentParent");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Category).WithMany().HasForeignKey("categoryId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.Comments).WithOne(x => x.Parent).HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
