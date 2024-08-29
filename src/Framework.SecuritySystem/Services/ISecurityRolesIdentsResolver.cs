namespace Framework.SecuritySystem;

public interface ISecurityRolesIdentsResolver
{
    IEnumerable<Guid> Resolve(DomainSecurityRule.RoleBaseSecurityRule securityRule);
}
