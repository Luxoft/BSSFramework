using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestRelativeEmployeeParentObjectMap : SampleSystemBaseMap<TestRelativeEmployeeParentObject>
{
    public override void Configure(EntityTypeBuilder<TestRelativeEmployeeParentObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestRelativeEmployeeParentObject");
        builder.HasMany(x => x.Children).WithOne(x => x.Master).HasForeignKey("masterId").OnDelete(DeleteBehavior.Cascade);
    }
}
