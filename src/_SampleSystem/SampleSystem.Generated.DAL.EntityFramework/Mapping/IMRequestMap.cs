using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.Employee;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class IMRequestMap : IEntityTypeConfiguration<IMRequest>
{
    public void Configure(EntityTypeBuilder<IMRequest> builder)
    {
        builder.ToTable("IMRequest");
        builder.Property(x => x.Message).HasMaxLength(50);
        builder.HasOne(x => x.OneToOneDetail).WithOne(x => x.Request).HasForeignKey<IMRequestDetail>("requestId").IsRequired().OnDelete(DeleteBehavior.Cascade);
    }
}
