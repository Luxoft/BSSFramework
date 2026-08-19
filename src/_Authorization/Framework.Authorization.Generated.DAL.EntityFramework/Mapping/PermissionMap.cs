using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Authorization.Generated.DAL.EntityFramework.Mapping;

public class PermissionMap : AuthBaseMap<Permission>
{
    public override void Configure(EntityTypeBuilder<Permission> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Comment).HasMaxLength(int.MaxValue);

        builder.HasOne(x => x.DelegatedFrom)
            .WithMany(x => x.DelegatedTo)
            .HasForeignKey("DelegatedFromId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Principal)
            .WithMany(x => x.Permissions)
            .HasForeignKey("PrincipalId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Role)
            .WithMany(x => x.Permissions)
            .HasForeignKey("RoleId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.ComplexProperty(
            x => x.Period,
            period =>
            {
                period.Property(x => x.EndDate).HasColumnName("periodendDate");
                period.Property(x => x.StartDate).HasColumnName("periodstartDate").IsRequired();
            });

        builder.HasMany(x => x.DelegatedTo)
            .WithOne(x => x.DelegatedFrom)
            .HasForeignKey("DelegatedFromId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Restrictions)
            .WithOne(x => x.Permission)
            .HasForeignKey("PermissionId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}
