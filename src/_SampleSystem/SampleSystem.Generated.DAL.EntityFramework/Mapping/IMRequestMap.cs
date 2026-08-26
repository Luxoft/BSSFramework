using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class IMRequestMap : SampleSystemBaseMap<IMRequest>
{
    public override void Configure(EntityTypeBuilder<IMRequest> builder)
    {
        base.Configure(builder);
        builder.HasOne(x => x.OneToOneDetail).WithOne(x => x.Request).HasForeignKey<IMRequestDetail>("requestId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
