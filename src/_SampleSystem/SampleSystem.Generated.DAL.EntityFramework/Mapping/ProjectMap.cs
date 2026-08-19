using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projects;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class ProjectMap : SampleSystemBaseMap<Project>
{
    public override void Configure(EntityTypeBuilder<Project> builder)
    {
        base.Configure(builder);
        builder.ToTable("Project", "dbo");
        builder.Property(x => x.Code).HasMaxLength(80).IsRequired();
        builder.Property(x => x.EndDate);
        builder.Property(x => x.PlannedEndDate).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.HasOne(x => x.BusinessUnit).WithMany(x => x.Projects).HasForeignKey("businessUnitId").IsRequired().OnDelete(DeleteBehavior.Restrict);
    }
}
