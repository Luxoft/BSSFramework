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
        builder.ToTable("SqlParserTestObj", "dbo");
        builder.Property(x => x.NotNullColumn).IsRequired();
        builder.Property(x => x.UniqueColumn).IsRequired();
        builder.HasIndex(x => x.UniqueColumn).IsUnique().HasDatabaseName("UIX_uniqueColumnAnotherSqlParserTestObj");
        builder.HasOne(typeof(SqlParserTestObj)).WithOne().HasForeignKey(typeof(AnotherSqlParserTestObj), nameof(AnotherSqlParserTestObj.Id));
    }
}
