using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class EmployeeSpecializationMap : SampleSystemBaseMap<EmployeeSpecialization>
{
    public override void Configure(EntityTypeBuilder<EmployeeSpecialization> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameEmployeeSpecialization");
    }
}
