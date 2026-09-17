using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestCustomContextSecurityObjMap : SampleSystemBaseMap<TestCustomContextSecurityObj>
{
    public override void Configure(EntityTypeBuilder<TestCustomContextSecurityObj> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestCustomContextSecurityObj");
        builder.Property(x => x.Name).IsRequired();
    }
}
