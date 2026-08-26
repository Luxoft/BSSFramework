using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ConcreteClassAMap : IEntityTypeConfiguration<ConcreteClassA>
{
    public void Configure(EntityTypeBuilder<ConcreteClassA> builder)
    {
        builder.Property(x => x.Age).IsRequired();
    }
}
