using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.EntityFramework.Mapping.Base;

using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Framework.Authorization.Generated.DAL.EntityFramework.Mapping;

public class BusinessRoleMap : AuthBaseMap<BusinessRole>
{
    public override void Configure(EntityTypeBuilder<BusinessRole> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Description);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasMany(x => x.Permissions)
            .WithOne(x => x.Role)
            .HasForeignKey("RoleId")
            .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict);
    }
}
