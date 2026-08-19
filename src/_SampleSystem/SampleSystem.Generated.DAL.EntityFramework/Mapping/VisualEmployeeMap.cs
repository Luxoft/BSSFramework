using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class VisualEmployeeMap : IEntityTypeConfiguration<VisualEmployee>
{
    public void Configure(EntityTypeBuilder<VisualEmployee> builder)
    {
        builder.ToTable("Employee", "dbo");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(x => x.NameEngFirstName).IsRequired();
        builder.ComplexProperty(x => x.NameEng, nameEng =>
        {
            nameEng.Property(x => x.FirstName).HasColumnName("nameEngfirstName").HasMaxLength(50);
            nameEng.Property(x => x.LastName).HasColumnName("nameEnglastName").HasMaxLength(50);
        });
    }
}
