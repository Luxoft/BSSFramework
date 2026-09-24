using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Authorization.Generated.DAL.EntityFramework.Mapping;

public class PermissionRestrictionMap : AuthBaseMap<PermissionRestriction>
{
    public override void Configure(EntityTypeBuilder<PermissionRestriction> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.SecurityContextId).IsRequired();

        builder.HasOne(x => x.Permission)
            .WithMany(x => x.Restrictions)
            .HasForeignKey("PermissionId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SecurityContextType)
            .WithMany()
            .HasForeignKey("SecurityContextTypeId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex("SecurityContextId", "PermissionId", "SecurityContextTypeId")
            .IsUnique()
            .HasDatabaseName("UIX_permission_securityContextId_securityContextTypePermissionRestriction");
    }
}
