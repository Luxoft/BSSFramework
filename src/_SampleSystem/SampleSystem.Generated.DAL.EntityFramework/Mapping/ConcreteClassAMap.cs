using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ConcreteClassAMap : IEntityTypeConfiguration<ConcreteClassA>
{
    public void Configure(EntityTypeBuilder<ConcreteClassA> builder)
    {
        builder.HasBaseType((Type?)null);
        builder.ToTable("ClassA");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(ClassA)).WithOne().HasForeignKey(typeof(ConcreteClassA), nameof(ConcreteClassA.Id)).IsRequired();
        builder.Ignore(x => x.Child);
        builder.Property(x => x.Age).HasColumnName("age").IsRequired();
    }
}
