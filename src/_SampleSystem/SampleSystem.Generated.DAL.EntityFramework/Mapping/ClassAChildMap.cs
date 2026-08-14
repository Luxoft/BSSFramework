using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ClassAChildMap : IEntityTypeConfiguration<ClassAChild>
{
    public void Configure(EntityTypeBuilder<ClassAChild> builder)
    {
        builder.ToTable("ClassAChild", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.IsFake);
        builder.HasOne(x => x.Parent).WithMany(x => x.Child).HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
