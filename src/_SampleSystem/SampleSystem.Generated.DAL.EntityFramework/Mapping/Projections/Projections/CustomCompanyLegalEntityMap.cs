using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Projections;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class CustomCompanyLegalEntityMap : IEntityTypeConfiguration<CustomCompanyLegalEntity>
{
    public void Configure(EntityTypeBuilder<CustomCompanyLegalEntity> builder)
    {
        builder.ToView("CompanyLegalEntity");
        //builder.HasKey(x => x.Id);
        //builder.Property(x => x.Id).ValueGeneratedOnAdd().IsRequired();
        //builder.Property(x => x.Code).IsRequired();
        //builder.SplitToTable("LegalEntityBase", "app", split =>
        //{
        //    split.Property(x => x.AribaStatusDescription).HasColumnName("aribaStatusdescription");
        //    split.Property(x => x.AribaStatusType).HasColumnName("aribaStatustype");
        //    split.Property(x => x.Name).HasColumnName("Name");
        //    split.Property(x => x.NameEnglish).HasColumnName("NameEnglish");
        //});
        //builder.HasOne(typeof(CompanyLegalEntity)).WithOne().HasForeignKey(typeof(CustomCompanyLegalEntity), nameof(CustomCompanyLegalEntity.Id)).OnDelete(DeleteBehavior.ClientCascade);
        //builder.Ignore(x => x.CurrentObj);
        //builder.Ignore(x => x.BaseObj);
    }
}
