using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeePositionMap : SampleSystemBaseMap<EmployeePosition>
{
    public override void Configure(EntityTypeBuilder<EmployeePosition> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.EnglishName).IsRequired();
        builder.Property(x => x.ExternalId).IsRequired();
        builder.HasOne(x => x.Location).WithMany().HasForeignKey("locationId").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("englishName", "locationId", "name").IsUnique().HasDatabaseName("UIX_englishName_location_nameEmployeePosition");
    }
}
