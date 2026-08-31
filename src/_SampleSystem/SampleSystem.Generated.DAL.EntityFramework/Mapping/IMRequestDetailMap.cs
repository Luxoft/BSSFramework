using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class IMRequestDetailMap : SampleSystemBaseMap<IMRequestDetail>
{
    public override void Configure(EntityTypeBuilder<IMRequestDetail> builder)
    {
        base.Configure(builder);
    }
}
