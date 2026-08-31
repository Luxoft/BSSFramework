using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.BU;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestBusinessUnitMap : IEntityTypeConfiguration<TestBusinessUnit>
{
    public void Configure(EntityTypeBuilder<TestBusinessUnit> builder)
    {
        builder.ToView(nameof(BusinessUnit));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.PeriodEndDate).HasColumnName("periodendDate").IsRequired();
        builder.HasOne(x => x.Her).WithMany().HasForeignKey("Id").IsRequired().OnDelete(DeleteBehavior.Restrict);
        builder.Property<System.Guid?>("parentId_TestBusinessUnit").HasColumnName("parentId");
        builder.HasOne(x => x.Parent_Auto).WithMany().HasForeignKey("parentId_TestBusinessUnit").IsRequired(false).OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("parentId_TestBusinessUnit").HasDatabaseName("IX_BusinessUnit_parentId_TestBusinessUnit");
        builder.HasMany(x => x.BusinessUnitEmployeeRoles).WithOne(x => x.BusinessUnit).HasForeignKey("businessUnitId_MiniBusinessUnitEmployeeRole");
    }
}
