namespace Framework.Authorization.SecuritySystem;

public interface ISecurityRoleSource
{
    IReadOnlyList<SecurityRole> SecurityRoles { get; }
}
