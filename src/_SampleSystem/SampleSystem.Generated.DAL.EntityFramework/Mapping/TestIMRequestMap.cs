using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestIMRequestMap : IEntityTypeConfiguration<TestIMRequest>
{
    public void Configure(EntityTypeBuilder<TestIMRequest> builder)
    {
        builder.ToTable("IMRequest", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Message);
        builder.HasOne(x => x.OneToOneDetail).WithOne(x => x.Request).HasForeignKey<TestIMRequestDetail>("requestId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
