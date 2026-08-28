using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.HRDepartment;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class TestDepartmentMap : IEntityTypeConfiguration<TestDepartment>
{
    public void Configure(EntityTypeBuilder<TestDepartment> builder)
    {
        builder.ToView("HRDepartment");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.HasOne(typeof(HRDepartment)).WithOne().HasForeignKey(typeof(TestDepartment), nameof(TestDepartment.Id));
        builder.Ignore(x => x.Location);
        builder.Ignore(x => x.Location_Auto);
    }
}
