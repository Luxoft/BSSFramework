using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestIMRequestDetailMap : IEntityTypeConfiguration<TestIMRequestDetail>
{
    public void Configure(EntityTypeBuilder<TestIMRequestDetail> builder)
    {
        builder.ToView(nameof(IMRequestDetail));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(IMRequestDetail)).WithOne().HasForeignKey(typeof(TestIMRequestDetail), nameof(TestIMRequestDetail.Id));
        builder.Property<System.Guid?>("requestId_TestIMRequestDetail").HasColumnName("requestId");
    }
}
