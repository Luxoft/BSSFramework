using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleSystem.Domain.NhFluentMapping;
namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;
public class PureFluentMappingObjectMap : IEntityTypeConfiguration<PureFluentMappingObject>
{
    public void Configure(EntityTypeBuilder<PureFluentMappingObject> builder)
    {
        builder.ToTable("PureFluentMappingObject", "app");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        builder.ComplexProperty(x => x.Period, period =>
        {
            period.Property(x => x.StartDate).HasColumnName("periodstartDate");
            period.Property(x => x.EndDate).HasColumnName("periodendDate");
        });
        builder.ComplexProperty(x => x.Period123, period =>
        {
            period.Property(x => x.StartDate).HasColumnName("period123startDate");
            period.Property(x => x.EndDate).HasColumnName("period123endDate");
        });
        builder.ComplexProperty(x => x.Period456, period =>
        {
            period.Property(x => x.StartDate).HasColumnName("period456startDate");
            period.Property(x => x.EndDate).HasColumnName("period456endDate");
        });
    }
}
