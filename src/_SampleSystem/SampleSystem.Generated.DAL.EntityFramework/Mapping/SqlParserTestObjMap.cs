using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class SqlParserTestObjMap : SampleSystemBaseMap<SqlParserTestObj>
{
    public override void Configure(EntityTypeBuilder<SqlParserTestObj> builder)
    {
        base.Configure(builder);
        builder.ToTable("SqlParserTestObj", "dbo");
        builder.Property(x => x.NotNullColumn);
        builder.Property(x => x.UniqueColumn);
        builder.HasIndex(x => x.UniqueColumn).IsUnique().HasDatabaseName("UIX_uniqueColumnSqlParserTestObj");
    }
}
