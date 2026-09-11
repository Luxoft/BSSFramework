using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class EmployeeRoleDegreeMap : SampleSystemBaseMap<EmployeeRoleDegree>
{
    public override void Configure(EntityTypeBuilder<EmployeeRoleDegree> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameEmployeeRoleDegree");
    }
}
