using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Directories;
using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestObjForNestedBaseMap : SampleSystemBaseMap<TestObjForNestedBase>
{
    public override void Configure(EntityTypeBuilder<TestObjForNestedBase> builder)
    {
        base.Configure(builder);
        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
        builder.ComplexProperty(x => x.Period, period => { period.Property(x => x.EndDate).HasColumnName("periodendDate"); period.Property(x => x.StartDate).HasColumnName("periodstartDate"); });
    }
}
