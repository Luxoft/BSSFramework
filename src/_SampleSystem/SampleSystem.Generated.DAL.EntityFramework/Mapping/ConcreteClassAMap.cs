using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ConcreteClassAMap : IEntityTypeConfiguration<ConcreteClassA>
{
    public void Configure(EntityTypeBuilder<ConcreteClassA> builder)
    {
        builder.ToTable("ClassA", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.Value).IsRequired();
        builder.HasMany(x => x.Child).WithOne().HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
