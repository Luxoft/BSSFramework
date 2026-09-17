using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Configuration.Generated.DAL.EntityFramework.Mapping;

public class SystemConstantMap : ConfigurationBaseMap<SystemConstant>
{
    public override void Configure(EntityTypeBuilder<SystemConstant> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(int.MaxValue);
        builder.Property(x => x.Value).HasMaxLength(int.MaxValue);

        builder.HasOne(x => x.Type)
            .WithMany()
            .HasForeignKey("TypeId")
            .IsRequired()
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder.HasIndex("Code").IsUnique().HasDatabaseName("UIX_codeSystemConstant");
    }
}
