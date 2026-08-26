using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestBusinessUnitMap : IEntityTypeConfiguration<TestBusinessUnit>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnit> builder)
    {
        builder.ToTable("BusinessUnit");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.PeriodEndDate).HasColumnName("periodendDate").IsRequired();
        builder.HasOne(x => x.Her).WithMany().HasForeignKey("Id").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Parent_Auto).WithMany().HasForeignKey("parentId").OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(x => x.BusinessUnitEmployeeRoles).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.ClientCascade);
    }
}
