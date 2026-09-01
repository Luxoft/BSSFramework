using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SqlParserTestObjContainerMap : SampleSystemBaseMap<SqlParserTestObjContainer>
{
    public override void Configure(EntityTypeBuilder<SqlParserTestObjContainer> builder)
    {
        base.Configure(builder);
        builder.ToTable("SqlParserTestObjContainer");
        builder.HasOne(x => x.IncludedObject).WithMany().HasForeignKey("includedObjectId").OnDelete(DeleteBehavior.Restrict);
    }
}
