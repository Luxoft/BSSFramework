using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class EmployeeRegistrationTypeMap : SampleSystemBaseMap<EmployeeRegistrationType>
{
    public override void Configure(EntityTypeBuilder<EmployeeRegistrationType> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.ExternalId).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique().HasDatabaseName("UIX_nameEmployeeRegistrationType");
    }
}
