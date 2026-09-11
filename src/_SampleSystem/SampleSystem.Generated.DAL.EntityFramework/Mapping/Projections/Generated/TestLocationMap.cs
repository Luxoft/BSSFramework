using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class TestLocationMap : IEntityTypeConfiguration<TestLocation>
{
    public void Configure(EntityTypeBuilder<TestLocation> builder)
    {
        builder.ToView(nameof(TestLocation));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("parentId_TestLocation").HasColumnName("parentId");
    }
}
