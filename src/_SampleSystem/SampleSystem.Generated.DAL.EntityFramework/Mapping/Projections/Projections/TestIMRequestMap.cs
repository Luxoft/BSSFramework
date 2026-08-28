using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestIMRequestMap : IEntityTypeConfiguration<TestIMRequest>
{
    public void Configure(EntityTypeBuilder<TestIMRequest> builder)
    {
        builder.ToView("IMRequest");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(IMRequest)).WithOne().HasForeignKey(typeof(TestIMRequest), nameof(TestIMRequest.Id));
        builder.HasOne(x => x.OneToOneDetail).WithOne().HasForeignKey(typeof(TestIMRequestDetail), "requestId_TestIMRequestDetail");
    }
}
