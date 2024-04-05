namespace Framework.SecuritySystem;

public interface ISecurityRoleSource
{
    IReadOnlyList<SecurityRole> SecurityRoles { get; }
}
