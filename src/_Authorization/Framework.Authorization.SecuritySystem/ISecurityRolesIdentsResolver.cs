using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface ISecurityRolesIdentsResolver
{
    IEnumerable<Guid> Resolve(SecurityRule.RoleBaseSecurityRule securityRule);
}
