using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestJobObjectMap : SampleSystemBaseMap<TestJobObject>
{
    public override void Configure(EntityTypeBuilder<TestJobObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestJobObject", "dbo");
    }
}
