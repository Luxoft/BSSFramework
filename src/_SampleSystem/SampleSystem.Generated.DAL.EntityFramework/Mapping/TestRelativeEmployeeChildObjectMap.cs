using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestRelativeEmployeeChildObjectMap : SampleSystemBaseMap<TestRelativeEmployeeChildObject>
{
    public override void Configure(EntityTypeBuilder<TestRelativeEmployeeChildObject> builder)
    {
        base.Configure(builder);
        builder.ToTable("TestRelativeEmployeeChildObject", "dbo");
        builder.HasOne(x => x.Employee).WithMany().HasForeignKey("employeeId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Master).WithMany(x => x.Children).HasForeignKey("masterId").OnDelete(DeleteBehavior.Restrict);
    }
}
