using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestIdComment;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCommentCategoryMap : SampleSystemBaseMap<TestCommentCategory>
{
    public override void Configure(EntityTypeBuilder<TestCommentCategory> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestCommentCategory");
        builder.Property(x => x.Name).IsRequired();
    }
}
