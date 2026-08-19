using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestRelativeEmployeeObjectMap : SampleSystemBaseMap<TestRelativeEmployeeObject>
{
    public override void Configure(EntityTypeBuilder<TestRelativeEmployeeObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestRelativeEmployeeObject", "dbo");
        builder.HasOne(x => x.EmployeeRef1).WithMany().HasForeignKey("employeeRef1Id").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.EmployeeRef2).WithMany().HasForeignKey("employeeRef2Id").OnDelete(DeleteBehavior.Restrict);
    }
}
