using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestLegacyEmployeeMap : IEntityTypeConfiguration<TestLegacyEmployee>
{
    public void Configure(EntityTypeBuilder<TestLegacyEmployee> builder)
    {
        builder.Property(x => x.Login).IsRequired();
        builder.HasOne(x => x.Role_Auto).WithMany().HasForeignKey("roleId").OnDelete(DeleteBehavior.Restrict);
    }
}
