using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Authorization.Generated.DAL.EntityFramework.Mapping;

public class PrincipalMap : AuthBaseMap<Principal>
{
    public override void Configure(EntityTypeBuilder<Principal> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasOne(x => x.RunAs)
            .WithMany()
            .HasForeignKey("RunAsId")
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);

        builder.HasMany(x => x.Permissions)
            .WithOne(x => x.Principal)
            .HasForeignKey("PrincipalId")
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
    }
}
