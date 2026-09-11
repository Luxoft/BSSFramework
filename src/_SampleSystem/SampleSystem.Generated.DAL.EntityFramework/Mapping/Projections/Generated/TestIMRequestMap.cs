using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestIMRequestMap : IEntityTypeConfiguration<TestIMRequest>
{
    public void Configure(EntityTypeBuilder<TestIMRequest> builder)
    {
        builder.ToView(nameof(TestIMRequest));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasOne(x => x.OneToOneDetail).WithOne().HasForeignKey(typeof(TestIMRequestDetail), "requestId_TestIMRequestDetail");
    }
}
