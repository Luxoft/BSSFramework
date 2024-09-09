namespace Framework.SecuritySystem;

public interface ISecurityRoleSource
{
    IReadOnlyList<FullSecurityRole> SecurityRoles { get; }

    FullSecurityRole GetSecurityRole(SecurityRole securityRole);

    FullSecurityRole GetSecurityRole(string name);

    FullSecurityRole GetSecurityRole(Guid id);

    IEnumerable<FullSecurityRole> GetRealRoles();
}
