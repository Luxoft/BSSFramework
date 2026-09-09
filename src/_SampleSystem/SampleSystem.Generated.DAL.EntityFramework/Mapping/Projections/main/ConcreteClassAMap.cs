using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Main;

public class ConcreteClassAMap : IEntityTypeConfiguration<ConcreteClassA>
{
    public void Configure(EntityTypeBuilder<ConcreteClassA> builder)
    {
        builder.HasBaseType((Type?)null);
        builder.ToView(nameof(ConcreteClassA));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("parentId_ConcreteClassA").HasColumnName("parentId");
        builder.HasMany(x => x.Child).WithOne().HasForeignKey("parentId_ConcreteClassA");
        builder.Property(x => x.Age).HasColumnName("age").IsRequired();
    }
}
