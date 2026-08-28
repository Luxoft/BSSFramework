using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class AnotherSqlParserTestObjMap : SampleSystemBaseMap<AnotherSqlParserTestObj>
{
    public override void Configure(EntityTypeBuilder<AnotherSqlParserTestObj> builder)
    {
        base.Configure(builder);
        builder.ToTable("SqlParserTestObj");
        builder.HasOne(typeof(SqlParserTestObj)).WithOne().HasForeignKey(typeof(AnotherSqlParserTestObj), nameof(AnotherSqlParserTestObj.Id));
        builder.Ignore(x => x.NotNullColumn);
        builder.Ignore(x => x.UniqueColumn);
    }
}
