using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping.Projections.Generated;

public class VisualEmployeeMap : IEntityTypeConfiguration<VisualEmployee>
{
    public void Configure(EntityTypeBuilder<VisualEmployee> builder)
    {
        builder.ToView(nameof(VisualEmployee));
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Ignore(x => x.NameEngFirstName);
        builder.ComplexProperty(x => x.NameEng, nameEng =>
        {
            nameEng.Property(x => x.FirstName).HasColumnName("nameEngfirstName").HasMaxLength(50);
            nameEng.Property(x => x.LastName).HasColumnName("nameEnglastName").HasMaxLength(50);
        });
    }
}
