using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class HerBusinessUnitMap : IEntityTypeConfiguration<HerBusinessUnit>
{
    public void Configure(EntityTypeBuilder<HerBusinessUnit> builder)
    {
        builder.ToTable("BusinessUnit", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Parent).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
    }
}
