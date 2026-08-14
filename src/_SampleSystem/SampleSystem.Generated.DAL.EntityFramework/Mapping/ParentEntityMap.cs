using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.UniqueByParent;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ParentEntityMap : SampleSystemBaseMap<ParentEntity>
{
    public override void Configure(EntityTypeBuilder<ParentEntity> builder)
    {
        base.Configure(builder);
        builder.ToTable("ParentEntity", "dbo");
    }
}
