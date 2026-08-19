using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestExceptObjectMap : SampleSystemBaseMap<TestExceptObject>
{
    public override void Configure(EntityTypeBuilder<TestExceptObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestExceptObject", "dbo");
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").OnDelete(DeleteBehavior.Restrict);
    }
}
