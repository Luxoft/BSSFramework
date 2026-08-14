using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ClassAMap : IEntityTypeConfiguration<ClassA>
{
    public void Configure(EntityTypeBuilder<ClassA> builder)
    {
        builder.ToTable("ClassA", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Value);
        builder.HasMany(x => x.Child).WithOne(x => x.Parent).HasForeignKey("parentId").OnDelete(DeleteBehavior.Cascade);
    }
}
