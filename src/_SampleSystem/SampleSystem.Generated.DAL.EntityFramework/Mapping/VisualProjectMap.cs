using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;
using SampleSystem.Domain.Projects;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class VisualProjectMap : IEntityTypeConfiguration<VisualProject>
{
    public void Configure(EntityTypeBuilder<VisualProject> builder)
    {
        builder.ToTable("Project", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(Project)).WithOne().HasForeignKey(typeof(VisualProject), nameof(VisualProject.Id));
        builder.Property(x => x.Code).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany().HasForeignKey("businessUnitId").OnDelete(DeleteBehavior.Restrict);
    }
}
