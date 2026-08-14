using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestIMRequestDetailMap : IEntityTypeConfiguration<TestIMRequestDetail>
{
    public void Configure(EntityTypeBuilder<TestIMRequestDetail> builder)
    {
        builder.ToTable("IMRequestDetail", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(x => x.Request).WithOne(x => x.OneToOneDetail).HasForeignKey<TestIMRequestDetail>("requestId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
