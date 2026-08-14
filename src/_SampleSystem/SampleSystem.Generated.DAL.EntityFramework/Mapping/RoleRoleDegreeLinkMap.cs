using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleSystem.Domain.Employee;

using SampleSystem.Generated.DAL.EntityFramework.Mapping.Base;

namespace SampleSystem.Generated.DAL.EntityFramework.Mapping;

public class RoleRoleDegreeLinkMap : SampleSystemBaseMap<RoleRoleDegreeLink>
{
    public override void Configure(EntityTypeBuilder<RoleRoleDegreeLink> builder)
    {
        base.Configure(builder);
        builder.ToTable("RoleRoleDegreeLink", "dbo");
        builder.HasOne(x => x.AnotherRole).WithMany().HasForeignKey("anotherRoleId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.Role).WithMany().HasForeignKey("roleId").OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(x => x.RoleDegree).WithMany().HasForeignKey("roleDegreeId").OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex("roleId", "roleDegreeId").IsUnique().HasDatabaseName("unilink_RoleRoleDegreeLink");
    }
}
