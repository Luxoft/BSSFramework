using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestIMRequestDetailMap : IEntityTypeConfiguration<TestIMRequestDetail>
{
    public void Configure(EntityTypeBuilder<TestIMRequestDetail> builder)
    {
        builder.ToView(nameof(TestIMRequestDetail));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("requestId_TestIMRequestDetail").HasColumnName("requestId");
    }
}
