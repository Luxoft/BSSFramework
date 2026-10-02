using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestIdComment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCommentMap : SampleSystemBaseMap<TestComment>
{
    public override void Configure(EntityTypeBuilder<TestComment> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestComment");
        builder.Property(x => x.Text);
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
