using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Main;

public class AnotherSqlParserTestObjMap : SampleSystemBaseMap<AnotherSqlParserTestObj>
{
    public override void Configure(EntityTypeBuilder<AnotherSqlParserTestObj> builder)
    {
        base.Configure(builder);
        builder.ToTable((string?)null);
        builder.ToView(nameof(AnotherSqlParserTestObj));
        builder.Property(x => x.Id).ValueGeneratedNever();
    }
}
