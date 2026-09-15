using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.EnversBug1676;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class WorkingCalendar1676Map : SampleSystemBaseMap<WorkingCalendar1676>
{
    public override void Configure(EntityTypeBuilder<WorkingCalendar1676> builder)
    {
        base.Configure(builder);
        builder.ToTable("WorkingCalendar1676");
        builder.Property(x => x.Name).IsRequired();
        builder.HasOne(x => x.Location).WithMany(x => x.Calendar).HasForeignKey("locationId").OnDelete(DeleteBehavior.Restrict);
    }
}
