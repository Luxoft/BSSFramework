using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class InformationMap : SampleSystemBaseMap<Information>
{
    public override void Configure(EntityTypeBuilder<Information> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Email).HasMaxLength(50);
    }
}
