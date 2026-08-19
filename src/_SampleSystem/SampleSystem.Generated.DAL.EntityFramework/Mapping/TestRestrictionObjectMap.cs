using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestRestrictionObjectMap : SampleSystemBaseMap<TestRestrictionObject>
{
    public override void Configure(EntityTypeBuilder<TestRestrictionObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestRestrictionObject", "dbo");
        builder.Property(x => x.RestrictionHandler).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
    }
}
