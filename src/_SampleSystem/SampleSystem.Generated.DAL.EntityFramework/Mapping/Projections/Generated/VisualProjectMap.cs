using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class VisualProjectMap : IEntityTypeConfiguration<VisualProject>
{
    public void Configure(EntityTypeBuilder<VisualProject> builder)
    {
        builder.ToView(nameof(VisualProject));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property<System.Guid?>("businessUnitId_VisualProject").HasColumnName("businessUnitId");
    }
}
